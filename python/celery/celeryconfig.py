import sys
reload(sys)
sys.setdefaultencoding("utf-8")

from kombu.serialization import registry
from kombu import serialization

registry.enable('json')
serialization.registry._decoders.pop("application/x-python-serialize")

BROKER_URL = 'amqp://guest:guest@localhost:5672//'

CELERY_IMPORTS = ('tasks', )

CELERY_RESULT_BACKEND = "database"
CELERY_RESULT_DBURI   = "mysql+mysqlconnector://celery:celery@localhost/celery?charset=utf8"

CELERY_ACCEPT_CONTENT = ['json']

CELERY_TASK_SERIALIZER = 'json'

CELERY_RESULT_SERIALIZER = 'json'

CELERY_TIMEZONE = 'Asia/Taipei'
