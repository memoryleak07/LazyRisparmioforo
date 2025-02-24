from transformers import AutoModelForSequenceClassification, AutoTokenizer
import torch
import os
from flask import Flask, request
from flask_restx import Api, Resource, fields

# Construct the path to the local model
# current_dir = os.path.dirname(__file__)
# local_model_path = os.path.join(current_dir, "..", "..", "autonlp-bank-transaction-classification-5521155")
# 
# # Set model and tokenizer
# model = AutoModelForSequenceClassification.from_pretrained(local_model_path)
# tokenizer = AutoTokenizer.from_pretrained(local_model_path)
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

@ns.route("/categories")
class Categories(Resource):
    def get(self):
        """Returns available transaction categories"""
        return id2label

transaction_request = api.model("TransactionRequest", {
    "transaction_text": fields.String(required=True, description="Transaction description")
})

@ns.route("/predict")
class Predict(Resource):
    @ns.expect(transaction_request)
    def post(self):
        """Predicts the category of a transaction"""

        # Get input request
        data = request.get_json()
        transaction_text = data.get("transaction_text", "")

        if not transaction_text:
            return {"error": "No transaction_text provided"}, 400

        # Tokenize input
        inputs = tokenizer(transaction_text, return_tensors="pt")

        # Run inference
        with torch.no_grad():
            outputs = model(**inputs)

        # Get softmax probabilities
        probs = torch.nn.functional.softmax(outputs.logits, dim=-1)

        # Get the category with the highest probability
        predicted_label_idx = torch.argmax(probs, dim=1).item()

        # Get category name from model's labels
        predicted_category = id2label.get(predicted_label_idx, f"Unknown ({predicted_label_idx})")
        confidence = probs[0][predicted_label_idx].item()

        return {
            "id": predicted_label_idx,
            "name": predicted_category.replace("Category.", ""),
            "confidence": round(confidence, 2)
        }

if __name__ == '__main__':
    port = int(os.environ.get('PORT', 51153))
    app.run(host='0.0.0.0', port=port)