from transformers import AutoModelForSequenceClassification, AutoTokenizer
import torch
import json
import os
from flask import Flask, request
from flask_restx import Api, Resource, fields
from category_mapping import *

with open(os.path.join("..", "..", "categories.json") , "r", encoding="utf-8") as file:
    consolidated_categories = json.load(file)

tokenizer = AutoTokenizer.from_pretrained("mgrella/autonlp-bank-transaction-classification-5521155")
model = AutoModelForSequenceClassification.from_pretrained("mgrella/autonlp-bank-transaction-classification-5521155")

# Get the model's label mapping
id2label = model.config.id2label  # Dictionary of {index: category_name}

app = Flask(__name__)
api = Api(
    app, 
    version="1.0", 
    title="Transaction Categorization API",
    description="API for categorizing bank transactions", 
    doc="/swagger")

ns = api.namespace("api")

def predict_category(transaction_text):
    """Runs model inference for a single transaction"""
    if not transaction_text:
        return { "error": "Empty transaction text" }

    # Tokenize input
    inputs = tokenizer(transaction_text, return_tensors="pt")

    # Run inference
    with torch.no_grad():
        outputs = model(**inputs)

    # Get softmax probabilities
    probs = torch.nn.functional.softmax(outputs.logits, dim=-1)

    # Get the category with the highest probability
    predicted_label_idx = torch.argmax(probs, dim=1).item()
    predicted_category = id2label.get(predicted_label_idx, f"Unknown ({predicted_label_idx})")
    confidence = probs[0][predicted_label_idx].item()

    # Get the consolidated category
    consolidated_category = CATEGORY_MAPPING.get(predicted_label_idx, 99) 

    return {
        "id": predicted_label_idx,
        "name": predicted_category.replace("Category.", ""),
        "confidence": round(confidence, 2),
        "consolidated": consolidated_category
    }

@ns.route("/categories")
class Categories(Resource):
    def get(self):
        """Returns available categories"""
        return id2label

input_request = api.model("InputRequest", {
    "input": fields.String(required=True, description="Input text")
})
@ns.route("/predict")
class Predict(Resource):
    @ns.expect(input_request)
    def post(self):
        """Predicts the category of an input text"""
        data = request.get_json()
        input = data.get("input", "")

        if not input:
            return {"error": "No input provided"}, 400

        prediction = predict_category(input)
        return prediction

input_batch_request = ns.model('InputBatchRequest', {
    'input': fields.List(fields.String, required=True, description='List of input texts')
})
@ns.route("/predict-batch")
class PredictBatch(Resource):
    @ns.expect(input_batch_request)
    def post(self):
        """Predicts the categories for a batch of input texts"""
        data = request.get_json()
        input = data.get("input", [])
        
        if not input:
            return {"error": "No input provided"}, 400

        predictions = [predict_category(text) for text in input]
        return predictions
    
if __name__ == '__main__':
    port = int(os.environ.get('PORT', 51153))
    app.run(host='0.0.0.0', port=port)