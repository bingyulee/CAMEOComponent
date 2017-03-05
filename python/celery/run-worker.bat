@echo off
celery worker -l info -n one@%h --autoscale=5,1