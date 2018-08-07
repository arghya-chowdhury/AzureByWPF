# Azure Continuous WebJob

Contains an application which can be published as webjob. It will run continously and monitor a queue for new item. Once found, it process the message and add a log entry of the message body.

## WebJob log
<!DOCTYPE HTML>
<!-- saved from url=(0250)https://studentgeneralstorage.blob.core.windows.net/azure-jobs-host-output/2518686188211736618_87b7383cdbf1461da5c9005597f9feb5 -->
<!DOCTYPE html PUBLIC "" ""><HTML><HEAD><META content="IE=11.0000" 
http-equiv="X-UA-Compatible">

<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
<META name="GENERATOR" content="MSHTML 11.00.10570.1001"></HEAD>
<BODY>
<PRE>{
  "Type": "FunctionCompleted",
  "EndTime": "2018-08-07T22:46:18.8263381+00:00",
  "ParameterLogs": {},
  "FunctionInstanceId": "77815a9c-32b6-4585-a8af-798277eef8e2",
  "Function": {
    "Id": "QueueTriggerWebJob.Functions.ProcessQueueMessage",
    "FullName": "QueueTriggerWebJob.Functions.ProcessQueueMessage",
    "ShortName": "Functions.ProcessQueueMessage",
    "Parameters": [
      {
        "Type": "QueueTrigger",
        "AccountName": "studentgeneralstorage",
        "QueueName": "student",
        "Name": "message"
      },
      {
        "Type": "ParameterDescriptor",
        "Name": "log"
      }
    ]
  },
  "Arguments": {
    "message": "Hello Friends! Its to test WebJob",
    "log": null
  },
  "Reason": "AutomaticTrigger",
  "ReasonDetails": "New queue message detected on 'student'.",
  "StartTime": "2018-08-07T22:46:18.4641118+00:00",
  "OutputBlob": {
    "ContainerName": "azure-webjobs-hosts",
    "BlobName": "output-logs/77815a9c32b64585a8af798277eef8e2.txt"
  },
  "ParameterLogBlob": {
    "ContainerName": "azure-webjobs-hosts",
    "BlobName": "output-logs/77815a9c32b64585a8af798277eef8e2.params.txt"
  },
  "LogLevel": "Info",
  "HostInstanceId": "9b827782-8ec5-4022-9201-767517edee97",
  "HostDisplayName": "QueueTriggerWebJob",
  "SharedQueueName": "azure-webjobs-host-21059ab070a3477f8115d614629ee3fa",
  "InstanceQueueName": "azure-webjobs-host-9b8277828ec540229201767517edee97",
  "Heartbeat": {
    "SharedContainerName": "azure-webjobs-hosts",
    "SharedDirectoryName": "heartbeats/21059ab070a3477f8115d614629ee3fa",
    "InstanceBlobName": "9b8277828ec540229201767517edee97",
    "ExpirationInSeconds": 45
  },
  "WebJobRunIdentifier": {
    "WebSiteName": "studentinfodevapp",
    "JobType": "Continuous",
    "JobName": "QueueTriggerWebJob",
    "RunId": ""
  }
}</PRE></BODY></HTML>

