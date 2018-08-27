<h2>ARM Template</h2>
"azuredeploy.json" is an Azure ARM template which can be used to provision a Virtual Network with two Virtual Machines and a Load balancer to divert traffic in round robin fashion. Click on "Deploy To Azure" button and provide your azure subscription for the deployment. Once provisioned you can go to the individual Windows Server VM and install IIS and change the default IIS home page located in inetpub/wwwroot. Now if you hit the load balancer public ip, you will notice that load balancer manages the traffic based on the virtual machines availability.

<a href="https://azuredeploy.net/" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Farghya-chowdhury%2FAzureSamples%2Fblob%2Fmaster%2FARMVMTemplate%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>

Virtual Network
![Network](https://github.com/arghya-chowdhury/AzureSamples/blob/master/ARMVMTemplate/Network.png)


