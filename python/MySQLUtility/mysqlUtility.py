#coding: utf-8
import pymysql.cursors
import json
import os.path
import io

'''
==========================================================================
Install: 
    Step 1: Install pymysql by excuting "pip install pymysql" in terminal
    Step 2: Set your MySQL database
    Step 3: Set the mysqlUtility_config.json file

Usage:
    Call mysqlUtility.Connect() at first
    Call mysqlUtility.Close() at last
==========================================================================
'''

class mysqlUtility:

    __connection = None

    '''
    功能：連線上db，所有功能呼叫前都必須先Connect才行
    '''
    @staticmethod
    def Connect(cursorclass = pymysql.cursors.DictCursor):
        if(mysqlUtility.__connection != None):
            mysqlUtility.__connection.close()
            mysqlUtility.__connection = None

        strConfigPath = mysqlUtility.getConfigFilePath()
        config = None
        if os.path.isfile(strConfigPath) == True:
            with open(strConfigPath, 'r') as jsonfile:
                config = json.loads(jsonfile.read(), encoding='utf-8')
                mysqlUtility.__connection = pymysql.connect( 
                            host = config["STR_HOST"],
                            user = config["STR_USER"],
                            password = config["STR_PASSWORD"],
                            db = config["STR_DB"],
                            charset = config["STR_CHARSET"],
                            cursorclass = cursorclass )
        else:
            print("[mysqlUtility.Connect] config file is not exist")

    '''
    功能：insert一筆新資料，如果資料庫中已有duplicate key，則會改成update，注意！目標table必須要設定哪個column是唯一的key(使用setColumnUnique)
    參數：
        strTable: insert的表格名稱
        dicInsertInfo: 要insert的資料，以dictionary方式儲存，key就是db的column名稱
    Ex:
        mysqlUtility.InsertOrUpdateWhenExist("my_table", {"strName": "Mike", "intAge": 30})
    '''
    @staticmethod
    def InsertOrUpdateWhenExist(strTable, dicInsertInfo):
        if(mysqlUtility.__connection == None):
            print("[mysqlUtility.InsertOrUpdateWhenExist] DB is not connected, please call mysqlUtility.Connect() at first")
            return

        cursor = mysqlUtility.__connection.cursor()
        strQuery = "INSERT INTO " + strTable
        lstStrTag = []
        lstStrValue = []
        lstStrUpdate = []
        for key, value in dicInsertInfo.iteritems():
            lstStrTag.append(key)
            if(isinstance(value, basestring)):
                lstStrValue.append("'" + value + "'")
            else:
                lstStrValue.append(str(value))
            lstStrUpdate.append(lstStrTag[-1] + "=" + lstStrValue[-1])
        strQuery = strQuery + "(" + ", ".join(lstStrTag) + ") VALUES(" + ", ".join(lstStrValue) + ") ON DUPLICATE KEY UPDATE " + ", ".join(lstStrUpdate) 
        try:
            cursor.execute(strQuery)
            mysqlUtility.__connection.commit()
        except:
            mysqlUtility.__connection.rollback()
            print("commit failed")

    '''
    功能：設定資料庫某個欄位是唯一的MySQL語法: ALTER TABLE mytbl ADD UNIQUE (columnName);
    參數：
        strTable: 要處理的table
        strColumnName: 要設為unique的column
    '''
    @staticmethod
    def setColumnUnique(strTable, strColumnName):
        if(mysqlUtility.__connection == None):
            print("[mysqlUtility.setColumnUnique] DB is not connected, please call mysqlUtility.Connect() at first")
            return

        cursor = mysqlUtility.__connection.cursor()
        strQuery = "ALTER TABLE " + strTable + " ADD UNIQUE (" + strColumnName + ")"
        cursor.execute(strQuery)

    '''
    功能：取得資料庫某個table的值;
    參數：
        lstStrSelectColumn: 要取得的column名稱，是string list
        strCondition: filter的條件，ex: strCategory='ART' AND isParsed<>1 
                      語法參考 https://www.w3schools.com/sql/sql_where.asp
    Ex:
        mysqlUtility.selectData("model_urls_webackers", ["strUrl"], strCondition = "strCategory='ART' AND isParsed<>1"))
    '''
    @staticmethod
    def selectData(strTable, lstStrSelectColumn = None, strCondition = None):
        if(mysqlUtility.__connection == None):
            print("[mysqlUtility.selectData] DB is not connected, please call mysqlUtility.Connect() at first")
            return

        cursor = mysqlUtility.__connection.cursor()
        strQuery = "SELECT "
        
        if(lstStrSelectColumn == None or len(lstStrSelectColumn) == 0):
            strQuery = strQuery + "*"
        else:
            strQuery = strQuery + ",".join(lstStrSelectColumn)
        strQuery = strQuery + " FROM " + strTable
        
        if(strCondition != None and strCondition != ""):
            strQuery = strQuery + " WHERE " + strCondition
        #print(strQuery)
        cursor.execute(strQuery)
        return list(cursor)

    @staticmethod 
    def Close():
        if(mysqlUtility.__connection == None):
            print("[mysqlUtility.Close] DB is not connected, please call mysqlUtility.Connect() at first")
        else:
            mysqlUtility.__connection.close()
            mysqlUtility.__connection = None

    @staticmethod
    def getConfigFilePath():
        return os.path.join(os.path.dirname(__file__)) + "/mysqlUtility_config.json"

'''
#[Example]
mysqlUtility.Connect()
print(mysqlUtility.selectData("model_urls_kickstarter", ["strUrl"], strCondition = "strCategory='Art' AND isNeedParse=0 AND intStatus=0"))
mysqlUtility.Close()
'''