import sys
sys.path.append("C:\Program Files (x86)\IronPython 2.7\Lib")

import json




#  Predefined global variables
ScriptName = ''
RoseApi = None
ServerConfig = json.loads('{}')
ScriptConfig = json.loads('{}')





def testFunc():
	global RoseApi
	ret = RoseApi.Execute("{'cmd':'select', 'collection':'samples.account'}")
	return ret



# Main function
def roseEntry():
	global RoseApi

	ret = RoseApi.Execute("{'cmd':'select', 'collection':'samples.account'}")
	return



# This function is called before processing the received request packet.
# If the return value is None, this request is not processed by the ROSE Server.
def beforeRequestHandling(context, messageBody):
	jsonData = json.loads(messageBody)

	cmd = jsonData['cmd']
	route = context.RawUrl
	remoteIPv4 = context.RemoteIPv4
	remoteIPv6 = context.RemoteIPv6


	if cmd == 'test':
		return onHandle_Test(context, jsonData)

	return messageBody



# This function is called before sending a response to the client.
def beforeResponseHandling(context, messageBody):
	return messageBody



# Custom request handlers
def onHandle_Test(context, query):
	RoseApi.Response(context, "hahaha")
	return None
