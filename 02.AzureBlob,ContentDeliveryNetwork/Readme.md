# Azure Blob Storage [![Build status](https://ci.appveyor.com/api/projects/status/v10ou5ihunbctr3k?svg=true)](https://ci.appveyor.com/project/arghya-chowdhury/azuresamples-smfd5)

<a href="https://azuredeploy.net/" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>
<a href="http://armviewer.azurewebsites.net/view?url=https%3A%2F%2Fraw.githubusercontent.com%2Farghya-chowdhury%2FAzureSamples%2Fmaster%2F01.AzureBlob%2CContentDeliveryNetwork%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>

The repository contains a Sample Wpf Application which can be manage containers and blobs with a very little configuration change. 
There are 4 values need to be set before the application starts working.
* StorageEndpointSuffix, example "core.windows.net"
* CdnEndpointSuffix, example "azureedge.net"
* EndPointName, example "quickstartstorage"
* & finally SASToken
    
The repository also contains an html page and javascript libraries provided by Microsoft. This would be the quickest way to look into your containers and blobs.

Both applications uses Azure Content Delivery Network to cache blob object at nearest pop location.

Sample app contains all CRUD operations of Blob Management 

## User Interface
![Blob Management](https://github.com/arghya-chowdhury/AzureSamples/blob/master/02.AzureBlob,ContentDeliveryNetwork/ClientInterface.png)
