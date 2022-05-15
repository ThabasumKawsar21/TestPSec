<?xml version="1.0"?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="VMSBL.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false"/>
        </sectionGroup>
    </configSections>
    <applicationSettings>
        <VMSBL.Properties.Settings>
            <setting name="VMSBL_localhost_AdminWebServices" serializeAs="String">
                <value>http://10.242.46.141/AdminWebServices/AdminWebService.asmx</value>
            </setting>
            <setting name="VMSBL_Adminwebservices_AdminWebServices" serializeAs="String">
                <value>http://adminwebservices/Adminwebservice.asmx</value>
            </setting>
        </VMSBL.Properties.Settings>
    </applicationSettings>
    <system.serviceModel>
        <bindings>
            <wsHttpBinding>
                <binding name="WSHttpBinding_IRequestUnifiedVASContract">
                    <security mode="None" />
                </binding>
                <binding name="WSHttpBinding_IRequestUnifiedVASContract1">
                    <security mode="Transport">
                        <transport clientCredentialType="None" />
                    </security>
                </binding>
            </wsHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://onecuatcoresrvs.cognizant.com/Messaging/OneCommunicator/Notification/VAS.svc"
                binding="wsHttpBinding" bindingConfiguration="WSHttpBinding_IRequestUnifiedVASContract"
                contract="OneCommunicatorService.IRequestUnifiedVASContract"
                name="WSHttpBinding_IRequestUnifiedVASContract">
                <identity>
                    <dns value="localhost" />
                </identity>
            </endpoint>
            <endpoint address="https://onecuatcoresrvs.cognizant.com/Messaging/OneCommunicator/Notification/VAS.svc"
                binding="wsHttpBinding" bindingConfiguration="WSHttpBinding_IRequestUnifiedVASContract1"
                contract="OneCommunicatorService.IRequestUnifiedVASContract"
                name="WSHttpBinding_IRequestUnifiedVASContract1" />
        </client>
    </system.serviceModel>
<startup><supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/></startup></configuration>
