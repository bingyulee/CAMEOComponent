import json
from celery import Celery

app = Celery()
app.config_from_object('celeryconfig')

@app.task()
def add(x, y):
    return { "result": x + y }

def multiply(x, y):
    return { "result": x * y }