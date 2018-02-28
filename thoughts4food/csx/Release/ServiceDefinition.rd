<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="thoughts4food" generation="1" functional="0" release="0" Id="7700c150-f86c-4b50-ada0-291ccec1936c" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="thoughts4foodGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebFacade:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/thoughts4food/thoughts4foodGroup/LB:WebFacade:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="CalcWorker:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapCalcWorker:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="CalcWorker:ContainerName" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapCalcWorker:ContainerName" />
          </maps>
        </aCS>
        <aCS name="CalcWorker:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapCalcWorker:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="CalcWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapCalcWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="CalcWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapCalcWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="NightWorker:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapNightWorker:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="NightWorker:ContainerName" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapNightWorker:ContainerName" />
          </maps>
        </aCS>
        <aCS name="NightWorker:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapNightWorker:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="NightWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapNightWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NightWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapNightWorkerInstances" />
          </maps>
        </aCS>
        <aCS name="WebFacade:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapWebFacade:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="WebFacade:ContainerName" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapWebFacade:ContainerName" />
          </maps>
        </aCS>
        <aCS name="WebFacade:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapWebFacade:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebFacade:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapWebFacade:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebFacadeInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/thoughts4food/thoughts4foodGroup/MapWebFacadeInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WebFacade:Endpoint1">
          <toPorts>
            <inPortMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCalcWorker:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorker/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapCalcWorker:ContainerName" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorker/ContainerName" />
          </setting>
        </map>
        <map name="MapCalcWorker:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorker/DataConnectionString" />
          </setting>
        </map>
        <map name="MapCalcWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapCalcWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorkerInstances" />
          </setting>
        </map>
        <map name="MapNightWorker:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/NightWorker/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapNightWorker:ContainerName" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/NightWorker/ContainerName" />
          </setting>
        </map>
        <map name="MapNightWorker:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/NightWorker/DataConnectionString" />
          </setting>
        </map>
        <map name="MapNightWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/NightWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNightWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/NightWorkerInstances" />
          </setting>
        </map>
        <map name="MapWebFacade:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapWebFacade:ContainerName" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade/ContainerName" />
          </setting>
        </map>
        <map name="MapWebFacade:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade/DataConnectionString" />
          </setting>
        </map>
        <map name="MapWebFacade:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebFacadeInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/WebFacadeInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="CalcWorker" generation="1" functional="0" release="0" software="C:\Users\Daniel\Documents\GitHub\thoughts4food\thoughts4food\csx\Release\roles\CalcWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="ContainerName" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;CalcWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CalcWorker&quot; /&gt;&lt;r name=&quot;NightWorker&quot; /&gt;&lt;r name=&quot;WebFacade&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/thoughts4food/thoughts4foodGroup/CalcWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="NightWorker" generation="1" functional="0" release="0" software="C:\Users\Daniel\Documents\GitHub\thoughts4food\thoughts4food\csx\Release\roles\NightWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="ContainerName" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NightWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CalcWorker&quot; /&gt;&lt;r name=&quot;NightWorker&quot; /&gt;&lt;r name=&quot;WebFacade&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/NightWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/thoughts4food/thoughts4foodGroup/NightWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/thoughts4food/thoughts4foodGroup/NightWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WebFacade" generation="1" functional="0" release="0" software="C:\Users\Daniel\Documents\GitHub\thoughts4food\thoughts4food\csx\Release\roles\WebFacade" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="ContainerName" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebFacade&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;CalcWorker&quot; /&gt;&lt;r name=&quot;NightWorker&quot; /&gt;&lt;r name=&quot;WebFacade&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/thoughts4food/thoughts4foodGroup/WebFacadeInstances" />
            <sCSPolicyUpdateDomainMoniker name="/thoughts4food/thoughts4foodGroup/WebFacadeUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/thoughts4food/thoughts4foodGroup/WebFacadeFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WebFacadeUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="CalcWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="NightWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="CalcWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="NightWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WebFacadeFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="CalcWorkerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="NightWorkerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WebFacadeInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="23537398-be2e-47c2-bacf-cefc4e2bff1a" ref="Microsoft.RedDog.Contract\ServiceContract\thoughts4foodContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="fc716087-a364-49c4-8bea-f411064d6e80" ref="Microsoft.RedDog.Contract\Interface\WebFacade:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/thoughts4food/thoughts4foodGroup/WebFacade:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>