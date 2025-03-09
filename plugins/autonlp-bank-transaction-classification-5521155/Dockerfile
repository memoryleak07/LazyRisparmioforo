FROM python:3.12-slim
WORKDIR /app
COPY categories.json /app/categories.json
COPY plugins/autonlp-bank-transaction-classification-5521155 .
RUN pip install -r requirements.txt
CMD ["python", "main.py"]

# docker build -f plugins/autonlp-bank-transaction-classification-5521155/Dockerfile .
