<?xml version="1.0" encoding="utf-8"?>
<root>
  <!-- 
    Microsoft ResX Schema 
    
    Version 2.0
    
    The primary goals of this format is to allow a simple XML format 
    that is mostly human readable. The generation and parsing of the 
    various data types are done through the TypeConverter classes 
    associated with the data types.
    
    Example:
    
    ... ado.net/XML headers & schema ...
    <resheader name="resmimetype">text/microsoft-resx</resheader>
    <resheader name="version">2.0</resheader>
    <resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
    <resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
    <data name="Name1"><value>this is my long string</value><comment>this is a comment</comment></data>
    <data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
    <data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
        <value>[base64 mime encoded serialized .NET Framework object]</value>
    </data>
    <data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
        <value>[base64 mime encoded string representing a byte array form of the .NET Framework object]</value>
        <comment>This is a comment</comment>
    </data>
                
    There are any number of "resheader" rows that contain simple 
    name/value pairs.
    
    Each data row contains a name, and value. The row also contains a 
    type or mimetype. Type corresponds to a .NET class that support 
    text/value conversion through the TypeConverter architecture. 
    Classes that don't support this are serialized and stored with the 
    mimetype set.
    
    The mimetype is used for serialized objects, and tells the 
    ResXResourceReader how to depersist the object. This is currently not 
    extensible. For a given mimetype the value must be set accordingly:
    
    Note - application/x-microsoft.net.object.binary.base64 is the format 
    that the ResXResourceWriter will generate, however the reader can 
    read any of the formats listed below.
    
    mimetype: application/x-microsoft.net.object.binary.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            : and then encoded with base64 encoding.
    
    mimetype: application/x-microsoft.net.object.soap.base64
    value   : The object must be serialized with 
            : System.Runtime.Serialization.Formatters.Soap.SoapFormatter
            : and then encoded with base64 encoding.

    mimetype: application/x-microsoft.net.object.bytearray.base64
    value   : The object must be serialized into a byte array 
            : using a System.ComponentModel.TypeConverter
            : and then encoded with base64 encoding.
    -->
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name="AssociateId" xml:space="preserve">
    <value>工号</value>
    <comment>Associate Id</comment>
  </data>
  <data name="CheckIn" xml:space="preserve">
    <value>签到</value>
    <comment>Check In</comment>
  </data>
  <data name="CheckOut" xml:space="preserve">
    <value>还卡</value>
    <comment>Check Out</comment>
  </data>
  <data name="City" xml:space="preserve">
    <value>城市</value>
    <comment>City</comment>
  </data>
  <data name="Clear" xml:space="preserve">
    <value>清空</value>
    <comment>Clear</comment>
  </data>
  <data name="FacilityName" xml:space="preserve">
    <value>设施名称</value>
    <comment>Facility</comment>
  </data>
  <data name="ForgotIDCard" xml:space="preserve">
    <value>忘记带卡</value>
    <comment>Forgot the ID Card</comment>
  </data>
  <data name="FromDate" xml:space="preserve">
    <value>开始日期</value>
    <comment>From Date</comment>
  </data>
  <data name="IssueTemporaryCard" xml:space="preserve">
    <value>发出临时卡</value>
    <comment>Issue Temporary Card Menu</comment>
  </data>
  <data name="IVS" xml:space="preserve">
    <value>身份验证</value>
    <comment>IVS Menu</comment>
  </data>
  <data name="Lost" xml:space="preserve">
    <value>丢失</value>
    <comment>Lost</comment>
  </data>
  <data name="OnsiteReturn" xml:space="preserve">
    <value>出差回来</value>
    <comment>Onsite Return</comment>
  </data>
  <data name="Reprint" xml:space="preserve">
    <value>重新打印</value>
    <comment>Re print</comment>
  </data>
  <data name="Search" xml:space="preserve">
    <value>搜索</value>
    <comment>Search</comment>
  </data>
  <data name="ToDate" xml:space="preserve">
    <value>截止日期</value>
    <comment>To Date</comment>
  </data>
  <data name="Actions" xml:space="preserve">
    <value>操作</value>
    <comment>Actions</comment>
  </data>
  <data name="AssociateName" xml:space="preserve">
    <value>姓名</value>
    <comment>Associate Name</comment>
  </data>
  <data name="BadgeNo" xml:space="preserve">
    <value>临时卡号</value>
    <comment>BadgeNo</comment>
  </data>
  <data name="BadgeStatus" xml:space="preserve">
    <value>临时卡状态</value>
    <comment>Badge Status</comment>
  </data>
  <data name="EmailID" xml:space="preserve">
    <value>邮箱</value>
    <comment>EmailID</comment>
  </data>
  <data name="Extension" xml:space="preserve">
    <value>分机</value>
    <comment>Extension</comment>
  </data>
  <data name="InDate" xml:space="preserve">
    <value>起始日期</value>
    <comment>InDate</comment>
  </data>
  <data name="InTime" xml:space="preserve">
    <value>签到时间</value>
    <comment>InTime</comment>
  </data>
  <data name="Location" xml:space="preserve">
    <value>办公地点</value>
    <comment>Location</comment>
  </data>
  <data name="Mobile" xml:space="preserve">
    <value>手机</value>
    <comment>Mobile</comment>
  </data>
  <data name="Name" xml:space="preserve">
    <value>姓名</value>
    <comment>Associate Name</comment>
  </data>
  <data name="OutTime" xml:space="preserve">
    <value>还卡时间</value>
    <comment>Out Time</comment>
  </data>
  <data name="btnCancel" xml:space="preserve">
    <value>返回</value>
    <comment>Back</comment>
  </data>
  <data name="btnConfirm" xml:space="preserve">
    <value>确认</value>
    <comment>Confirm</comment>
  </data>
  <data name="lblCommitting" xml:space="preserve">
    <value>确认签到临时卡将会制作好，相关信息会发送给员工的主管。员工信息是否核对正确？</value>
    <comment>Committing Words</comment>
  </data>
  <data name="lblConfirm" xml:space="preserve">
    <value>确认请按“确认”键，其它请按“返回”键</value>
    <comment>Confirm</comment>
  </data>
  <data name="lblInformation" xml:space="preserve">
    <value>提示！</value>
    <comment>Information</comment>
  </data>
  <data name="lblReminderCommitting" xml:space="preserve">
    <value>确认提醒邮件会发送给员工告知离开公司时归还临时卡。确认要发送邮件？</value>
    <comment>Committing ReminderWords</comment>
  </data>
  <data name="ReminderMail" xml:space="preserve">
    <value>邮件提醒</value>
    <comment>Reminder Mail</comment>
  </data>
  <data name="lblAmber" xml:space="preserve">
    <value>使用中</value>
    <comment>Inside the Premises</comment>
  </data>
  <data name="lblEmployeeHeader" xml:space="preserve">
    <value>员工信息</value>
    <comment>Employee Details</comment>
  </data>
  <data name="lblGreen" xml:space="preserve">
    <value>已返还</value>
    <comment>Successful check out</comment>
  </data>
  <data name="lblLegend" xml:space="preserve">
    <value>状态标识</value>
    <comment>Legend</comment>
  </data>
  <data name="lblManagerHeader" xml:space="preserve">
    <value>经理信息</value>
    <comment>Manager Details</comment>
  </data>
  <data name="SlNo" xml:space="preserve">
    <value>编号</value>
    <comment>SL.No</comment>
  </data>
  <data name="Status" xml:space="preserve">
    <value>状态</value>
    <comment>Status</comment>
  </data>
  <data name="btnView" xml:space="preserve">
    <value>查看</value>
    <comment>View</comment>
  </data>
  <data name="OutDate" xml:space="preserve">
    <value>还卡日期</value>
    <comment>Out Date</comment>
  </data>
  <data name="Reason" xml:space="preserve">
    <value>原因</value>
    <comment>Reason</comment>
  </data>
  <data name="DateRange" xml:space="preserve">
    <value>请选择7天内的日期（含今天）</value>
    <comment>Please select a date within 7 days from today</comment>
  </data>
  <data name="DateValid" xml:space="preserve">
    <value>截止日期必须在起始日期之后</value>
    <comment>To Date has to be greater than From Date</comment>
  </data>
  <data name="EnterAssociateId" xml:space="preserve">
    <value>请输入员工号</value>
    <comment>Please Enter AssociateID</comment>
  </data>
  <data name="ErrorMessageCheckOut" xml:space="preserve">
    <value>出错啦！</value>
    <comment>An Error has occured while checking out</comment>
  </data>
  <data name="InValidAssociateId" xml:space="preserve">
    <value>该工号不存在</value>
    <comment>Associate ID does not exist</comment>
  </data>
  <data name="lblModuleValue" xml:space="preserve">
    <value>制作临时员工卡</value>
    <comment>Issue Temporary ID Card</comment>
  </data>
  <data name="ReminderNotification" xml:space="preserve">
    <value>提醒邮件已发送</value>
    <comment>Reminder Mail has been sent to the Associate</comment>
  </data>
  <data name="TempCardReturned" xml:space="preserve">
    <value>员工</value>
    <comment>Temporary ID card Returned successfully for the Associate</comment>
  </data>
  <data name="Address" xml:space="preserve">
    <value>地址</value>
    <comment>Address</comment>
  </data>
  <data name="BloodGroup" xml:space="preserve">
    <value>血型:</value>
    <comment>BloodGroup</comment>
  </data>
  <data name="CompanyName" xml:space="preserve">
    <value>高知特信息技术（上海）有限公司</value>
    <comment>Cognizant Technology Solutions</comment>
  </data>
  <data name="EmercengyContact" xml:space="preserve">
    <value>紧急联系方式:</value>
    <comment>EmercengyContact</comment>
  </data>
  <data name="Facility" xml:space="preserve">
    <value>办公地点</value>
    <comment>Facility Name</comment>
  </data>
  <data name="lblArea" xml:space="preserve">
    <value>国家，城市，邮编</value>
    <comment>City Pin Code, Country</comment>
  </data>
  <data name="lblBottomCaption" xml:space="preserve">
    <value>版权所有， 推荐使用IE7.0 及以上版本的具有1024x768分辨率及以上的浏览器</value>
  </data>
  <data name="lblCardNoIssuedAlert" xml:space="preserve">
    <value>今天没有申请临时卡的记录</value>
    <comment>There hasn't been any ODI Card issued today</comment>
  </data>
  <data name="lblCheckoutError" xml:space="preserve">
    <value>出错啦！</value>
    <comment>An Error has occured while checking out</comment>
  </data>
  <data name="lblEmpPass" xml:space="preserve">
    <value>临时卡号</value>
    <comment>ODI Pass</comment>
  </data>
  <data name="lblSecurityInValid" xml:space="preserve">
    <value>没有权限访问</value>
    <comment>Security ID is not tagged to a Facility</comment>
  </data>
  <data name="lblTempId" xml:space="preserve">
    <value>员工临时卡</value>
    <comment>TEMPORARY IDENTITY CARD</comment>
  </data>
  <data name="lblToDateValidation" xml:space="preserve">
    <value>请选择有效的截止时间</value>
    <comment>Please select a valid Date in the 'ToDate' field</comment>
  </data>
  <data name="lblValidfrom" xml:space="preserve">
    <value>有效期从</value>
    <comment>VALID FROM</comment>
  </data>
  <data name="lblValidTo" xml:space="preserve">
    <value>至</value>
    <comment>VALID TO</comment>
  </data>
  <data name="NoRecordFound" xml:space="preserve">
    <value>没有符合条件的访客</value>
    <comment>No Record Found</comment>
  </data>
  <data name="Notification" xml:space="preserve">
    <value>该员工已申请临时卡</value>
    <comment>Associate has been issued a Temporary ID Card</comment>
  </data>
  <data name="Print" xml:space="preserve">
    <value>打印</value>
    <comment>Print</comment>
  </data>
  <data name="PrinterJammed" xml:space="preserve">
    <value>打印机卡纸</value>
    <comment>PrinterJammed</comment>
  </data>
  <data name="SelectReason" xml:space="preserve">
    <value>请选择原因</value>
    <comment>Select Reason</comment>
  </data>
  <data name="Under" xml:space="preserve">
    <value>在</value>
    <comment>Under</comment>
  </data>
  <data name="ActualOutTime" xml:space="preserve">
    <value>实际还卡时间</value>
    <comment>Actual Out Time</comment>
  </data>
  <data name="btnBackToRequests" xml:space="preserve">
    <value>返回申请页面</value>
    <comment>Go to Requests</comment>
  </data>
  <data name="btnExport" xml:space="preserve">
    <value>导出</value>
    <comment>Export</comment>
  </data>
  <data name="Company" xml:space="preserve">
    <value>公司</value>
    <comment>Company</comment>
  </data>
  <data name="Date" xml:space="preserve">
    <value>日期</value>
    <comment>Date</comment>
  </data>
  <data name="Host" xml:space="preserve">
    <value>申请人</value>
    <comment>Host</comment>
  </data>
  <data name="imgcalenderControl" xml:space="preserve">
    <value>查看日历控件</value>
    <comment>To View the calender control</comment>
  </data>
  <data name="lblExceedOutTime" xml:space="preserve">
    <value>预计还卡时间</value>
    <comment>Expected Out Time</comment>
  </data>
  <data name="lblExpectedOutTime" xml:space="preserve">
    <value>预计还卡时间</value>
    <comment>Expected Out Time</comment>
  </data>
  <data name="lblNoRelevantRequest" xml:space="preserve">
    <value>没有相关申请？请点击这里</value>
    <comment>No Relevant Requests?Click Here</comment>
  </data>
  <data name="lblSearchOR" xml:space="preserve">
    <value>或</value>
    <comment>(OR)</comment>
  </data>
  <data name="lblVisit" xml:space="preserve">
    <value>是否来过高知特？</value>
    <comment>Have you visited any of the Cognizant facilities before?</comment>
  </data>
  <data name="RefreshDate" xml:space="preserve">
    <value>刷新</value>
    <comment>Refresh Date</comment>
  </data>
  <data name="searchByName" xml:space="preserve">
    <value>搜索姓名/公司/职位/手机/卡号</value>
    <comment>Search By Name, Organization, Designation, Mobile No, Badge No</comment>
  </data>
  <data name="VisitorType" xml:space="preserve">
    <value>访客类型</value>
    <comment>VisitorType</comment>
  </data>
  <data name="Yettoarrive" xml:space="preserve">
    <value>访客未到</value>
    <comment>Yettoarrive</comment>
  </data>
  <data name="BadgeStatusUpdate" xml:space="preserve">
    <value>更新卡状态</value>
    <comment>Badge status update</comment>
  </data>
  <data name="Comments" xml:space="preserve">
    <value>注释</value>
    <comment>Comments</comment>
  </data>
  <data name="Designation" xml:space="preserve">
    <value>职位</value>
    <comment>Designation</comment>
  </data>
  <data name="Update" xml:space="preserve">
    <value>更新</value>
    <comment>Update</comment>
  </data>
  <data name="ViewDetails" xml:space="preserve">
    <value>查看详细信息</value>
    <comment>View Details</comment>
  </data>
  <data name="VisitorId" xml:space="preserve">
    <value>访客编号</value>
    <comment>VisitorId</comment>
  </data>
  <data name="lblClickHere" xml:space="preserve">
    <value>点击这里</value>
    <comment>Click Here</comment>
  </data>
  <data name="lblExpressCheckin" xml:space="preserve">
    <value>输入临时卡号</value>
    <comment>Enter Express Check-In No</comment>
  </data>
  <data name="ContactAddress" xml:space="preserve">
    <value>来访期间联系地址</value>
    <comment>Contact Address  while visiting</comment>
  </data>
  <data name="lblEmergencyContact" xml:space="preserve">
    <value>所在地紧急联系电话</value>
    <comment>Emergency Contact in Visiting City</comment>
  </data>
  <data name="lblEmployeeID" xml:space="preserve">
    <value>工号</value>
    <comment>ID</comment>
  </data>
  <data name="lblEquipmentsPermitted" xml:space="preserve">
    <value>携带设备</value>
    <comment>Equipments Permitted</comment>
  </data>
  <data name="lblEquipmentType" xml:space="preserve">
    <value>设备类型</value>
    <comment>EquipmentType</comment>
  </data>
  <data name="btnNo" xml:space="preserve">
    <value>否</value>
    <comment>No</comment>
  </data>
  <data name="btnYes" xml:space="preserve">
    <value>是</value>
    <comment>Yes</comment>
  </data>
  <data name="BadgeLost" xml:space="preserve">
    <value>临时卡丢失.</value>
    <comment>Badge Lost. Check Out Successful</comment>
  </data>
  <data name="BadgeReturned" xml:space="preserve">
    <value>已还卡. 访客离开</value>
    <comment>Badge Returned. Check Out Successful</comment>
  </data>
  <data name="First" xml:space="preserve">
    <value>首页</value>
    <comment>First</comment>
  </data>
  <data name="Last" xml:space="preserve">
    <value>最后一页</value>
    <comment>Last</comment>
  </data>
  <data name="lblInvalidCheckinCode" xml:space="preserve">
    <value>签到码无效</value>
    <comment>Invalid Check In Code</comment>
  </data>
  <data name="Next" xml:space="preserve">
    <value>下页</value>
    <comment>Next</comment>
  </data>
  <data name="NoOpenRequest" xml:space="preserve">
    <value>没有对应这个办公地点的申请签到码</value>
    <comment>There are no open requests with the Check In Code entered for this facility</comment>
  </data>
  <data name="Previous" xml:space="preserve">
    <value>上一页</value>
    <comment>Previous</comment>
  </data>
  <data name="ReprintComments" xml:space="preserve">
    <value>注释</value>
    <comment>ReprintComments</comment>
  </data>
  <data name="Reprintsuccessful" xml:space="preserve">
    <value>成功重新打印</value>
    <comment>Reprintsuccessful</comment>
  </data>
  <data name="Selectsearch" xml:space="preserve">
    <value>请至少选择一项查询条件</value>
    <comment>Select atleast one search option</comment>
  </data>
  <data name="VisitorBadgeGenerated" xml:space="preserve">
    <value>发卡成功</value>
    <comment>Visitor's Badge is successfully generated</comment>
  </data>
  <data name="Visitorcheckinsuccessful" xml:space="preserve">
    <value>访客签到</value>
    <comment>Visitor check in successful</comment>
  </data>
  <data name="SelectReprintCommand" xml:space="preserve">
    <value>请选择重新打印的注释</value>
    <comment>Select a comment for reprinting the badge</comment>
  </data>
  <data name="EquipmentPermitted" xml:space="preserve">
    <value>携带设备 :</value>
    <comment>EquipmentPermitted</comment>
  </data>
  <data name="From" xml:space="preserve">
    <value>开始时间</value>
    <comment>From</comment>
  </data>
  <data name="HostSignature" xml:space="preserve">
    <value>申请人签名:</value>
    <comment>Host Signature</comment>
  </data>
  <data name="lblBadgeNote" xml:space="preserve">
    <value>离开时请归还临时卡至保卫处</value>
  </data>
  <data name="Logo" xml:space="preserve">
    <value>标志</value>
    <comment>Logo</comment>
  </data>
  <data name="Make" xml:space="preserve">
    <value>制造商</value>
    <comment>Make</comment>
  </data>
  <data name="Model" xml:space="preserve">
    <value>型号</value>
    <comment>Model</comment>
  </data>
  <data name="NoEquipmentPermitted" xml:space="preserve">
    <value>不允许携带设备</value>
    <comment>No Equipment Permitted</comment>
  </data>
  <data name="Organization" xml:space="preserve">
    <value>公司</value>
    <comment>Organization</comment>
  </data>
  <data name="SerialNo" xml:space="preserve">
    <value>序列号</value>
    <comment>Serial No</comment>
  </data>
  <data name="Time" xml:space="preserve">
    <value>时间:</value>
    <comment>Time</comment>
  </data>
  <data name="To" xml:space="preserve">
    <value>结束时间</value>
    <comment>To</comment>
  </data>
  <data name="VisitorsPass" xml:space="preserve">
    <value>访客通行编号 :</value>
    <comment>Visitors Pass # :</comment>
  </data>
  <data name="VMS" xml:space="preserve">
    <value>访客管理</value>
    <comment>VMS</comment>
  </data>
  <data name="KindlyFollow" xml:space="preserve">
    <value>请遵循背面的指示</value>
    <comment>Kindly follow the instructions provided on the reverse</comment>
  </data>
  <data name="lblEmergencyContactInstr" xml:space="preserve">
    <value>紧急情况，请联系保卫处：</value>
    <comment>In case of any emergency please contact the security at</comment>
  </data>
  <data name="Note" xml:space="preserve">
    <value>注意</value>
    <comment>Note</comment>
  </data>
  <data name="In" xml:space="preserve">
    <value>访问中</value>
    <comment>In</comment>
  </data>
  <data name="Out" xml:space="preserve">
    <value>已离开</value>
    <comment>Out</comment>
  </data>
  <data name="SelectStatus" xml:space="preserve">
    <value>请选择状态</value>
    <comment>Select Status</comment>
  </data>
  <data name="Country" xml:space="preserve">
    <value>国家</value>
    <comment>Country</comment>
  </data>
  <data name="SelectEquipmentType" xml:space="preserve">
    <value>请选择设备</value>
    <comment>Please Select any of the Equipment Type</comment>
  </data>
  <data name="VisitLocationInformation" xml:space="preserve">
    <value>访问地信息</value>
    <comment>Visit Location Information</comment>
  </data>
  <data name="FromTime" xml:space="preserve">
    <value>开始时间</value>
    <comment>From Time</comment>
  </data>
  <data name="HostContactNumber" xml:space="preserve">
    <value>申请人联系电话</value>
    <comment>申请人联系电话</comment>
  </data>
  <data name="InvalidTime" xml:space="preserve">
    <value>时间无效</value>
    <comment>InvalidTime</comment>
  </data>
  <data name="Others" xml:space="preserve">
    <value>如果选择“其他”，也请填写来访类型</value>
    <comment>If Others Specify</comment>
  </data>
  <data name="RequiredFromDate" xml:space="preserve">
    <value>请输入来访日期</value>
    <comment>Enter From Date</comment>
  </data>
  <data name="RequiredToDate" xml:space="preserve">
    <value>请输入结束日期</value>
    <comment>Enter To Date</comment>
  </data>
  <data name="SearchHostName" xml:space="preserve">
    <value>搜索申请人</value>
    <comment>Search HostName</comment>
  </data>
  <data name="SelectCity" xml:space="preserve">
    <value>请选择城市</value>
    <comment>Select City</comment>
  </data>
  <data name="SelectCountry" xml:space="preserve">
    <value>请选择国家</value>
    <comment>Select Country</comment>
  </data>
  <data name="SelectHost" xml:space="preserve">
    <value>请选择申请人</value>
    <comment>Select Host</comment>
  </data>
  <data name="SelectVisitorType" xml:space="preserve">
    <value>请选择访客类型</value>
    <comment>Select Visitor Type</comment>
  </data>
  <data name="TimeEmpty" xml:space="preserve">
    <value>未输入时间</value>
    <comment>Time is not entered</comment>
  </data>
  <data name="ToTime" xml:space="preserve">
    <value>结束时间</value>
    <comment>To Time</comment>
  </data>
  <data name="EmailCheck" xml:space="preserve">
    <value>请输入有效的邮箱</value>
    <comment>Enter valid Email ID</comment>
  </data>
  <data name="EnterFirstName" xml:space="preserve">
    <value>请输入访客名字</value>
    <comment>EnterFirstName</comment>
  </data>
  <data name="EnterLastName" xml:space="preserve">
    <value>请输入访客姓氏</value>
    <comment>Enter Last Name</comment>
  </data>
  <data name="FeMale" xml:space="preserve">
    <value>女</value>
    <comment>FeMale</comment>
  </data>
  <data name="FirstName" xml:space="preserve">
    <value>名字</value>
    <comment>First Name</comment>
  </data>
  <data name="Gender" xml:space="preserve">
    <value>性别</value>
    <comment>Gender</comment>
  </data>
  <data name="GenerateBadge" xml:space="preserve">
    <value>是否制作临时卡？</value>
    <comment>Do you want to generate badge?</comment>
  </data>
  <data name="LastName" xml:space="preserve">
    <value>姓氏</value>
    <comment>LastName</comment>
  </data>
  <data name="Male" xml:space="preserve">
    <value>男</value>
    <comment>Male</comment>
  </data>
  <data name="MandatoryField" xml:space="preserve">
    <value>必填项</value>
    <comment>Indicates Mandatory Field</comment>
  </data>
  <data name="MultipleEntries" xml:space="preserve">
    <value>多位访客</value>
    <comment>Multiple Entries</comment>
  </data>
  <data name="NativeCountry" xml:space="preserve">
    <value>国籍</value>
    <comment>Native Country</comment>
  </data>
  <data name="ReqNativecountry" xml:space="preserve">
    <value>请选择国籍</value>
    <comment>Select Native country</comment>
  </data>
  <data name="Select" xml:space="preserve">
    <value>请选择</value>
    <comment>Select</comment>
  </data>
  <data name="VisitorInformation" xml:space="preserve">
    <value>访客信息</value>
    <comment>Visitor Information</comment>
  </data>
  <data name="DuplicateVisitor" xml:space="preserve">
    <value>访客信息已存在</value>
    <comment>Visitor Information already available</comment>
  </data>
  <data name="Requestnotcancelled" xml:space="preserve">
    <value>未删除</value>
    <comment>Request is not cancelled</comment>
  </data>
  <data name="SavedForFutureSubmission" xml:space="preserve">
    <value>保存成功</value>
    <comment>SavedForFutureSubmission</comment>
  </data>
  <data name="SubmitMandatory" xml:space="preserve">
    <value>请提交申请</value>
    <comment>Please Submit a Request</comment>
  </data>
  <data name="SubmittedSuccessfully" xml:space="preserve">
    <value>提交成功！</value>
    <comment>Submitted Successfully</comment>
  </data>
  <data name="TimeExtend" xml:space="preserve">
    <value>延时成功</value>
    <comment>TimeExtend</comment>
  </data>
  <data name="UpdatedSuccessfully" xml:space="preserve">
    <value>更新成功</value>
    <comment>Updated Successfully</comment>
  </data>
  <data name="UpdationFailed" xml:space="preserve">
    <value>未更新成功</value>
    <comment>Updation Failed</comment>
  </data>
  <data name="Back" xml:space="preserve">
    <value>后退</value>
    <comment>Back</comment>
  </data>
  <data name="AssociateDetailsPopUpCaption" xml:space="preserve">
    <value>请输入员工号或者姓名</value>
    <comment>Enter either Associate Id or Associate Name</comment>
  </data>
  <data name="AssociateList" xml:space="preserve">
    <value>员工名单</value>
    <comment>Associate List</comment>
  </data>
  <data name="Cancel" xml:space="preserve">
    <value>取消</value>
    <comment>Cancel</comment>
  </data>
  <data name="CancelConfirmation" xml:space="preserve">
    <value>确定取消？</value>
    <comment>Are you sure you want to cancel?</comment>
  </data>
  <data name="GenerateBadgeRequest" xml:space="preserve">
    <value>制作临时卡</value>
    <comment>Generate Badge</comment>
  </data>
  <data name="Reset" xml:space="preserve">
    <value>重置</value>
    <comment>Reset</comment>
  </data>
  <data name="Save" xml:space="preserve">
    <value>保存</value>
    <comment>Save</comment>
  </data>
  <data name="SearchUserDetails" xml:space="preserve">
    <value>搜索用户信息</value>
    <comment>Search User Details</comment>
  </data>
  <data name="Submit" xml:space="preserve">
    <value>提交</value>
    <comment>Submit</comment>
  </data>
  <data name="UploadAndClose" xml:space="preserve">
    <value>上传关闭</value>
    <comment>Upload &amp; Close</comment>
  </data>
  <data name="UploadPhoto" xml:space="preserve">
    <value>上传照片</value>
    <comment>Upload Photo</comment>
  </data>
  <data name="WebCam" xml:space="preserve">
    <value>拍照</value>
    <comment>WebCam</comment>
  </data>
  <data name="EquipmentType" xml:space="preserve">
    <value>设备类型</value>
    <comment>EquipmentType</comment>
  </data>
  <data name="Upload" xml:space="preserve">
    <value>上传</value>
    <comment>Upload</comment>
  </data>
  <data name="VisitorImage" xml:space="preserve">
    <value>访客照片</value>
    <comment>Visitor Image</comment>
  </data>
  <data name="Chinese" xml:space="preserve">
    <value>Chinese</value>
    <comment>Chinese</comment>
  </data>
  <data name="English" xml:space="preserve">
    <value>English</value>
    <comment>English</comment>
  </data>
  <data name="ErrorMessage" xml:space="preserve">
    <value>出错啦！</value>
    <comment>Error Message</comment>
  </data>
  <data name="Help" xml:space="preserve">
    <value>帮助</value>
    <comment>Help</comment>
  </data>
  <data name="Home" xml:space="preserve">
    <value>主页</value>
    <comment>Home</comment>
  </data>
  <data name="Logout" xml:space="preserve">
    <value>退出</value>
    <comment>Logout</comment>
  </data>
  <data name="OfferedLanguage" xml:space="preserve">
    <value>语言</value>
    <comment>PS offered in:</comment>
  </data>
  <data name="PurposeList" xml:space="preserve">
    <value>&lt;PurposeList&gt;&lt;Purpose text="请选择访客类型" value="Select"/&gt;
  &lt;Purpose text="客户" value="Clients"/&gt;
  &lt;Purpose text="供应商" value="Vendor"/&gt;
  &lt;Purpose text="来宾" value="Guests"/&gt;
  &lt;Purpose text="贵宾" value="Imp/VIP Visitor"/&gt;
  &lt;Purpose text="应聘者" value="Interview Candidate"/&gt;
  &lt;Purpose text="前雇员" value="Former Employee"/&gt;
  &lt;Purpose text="家庭来访" value="Family Visit"/&gt;
  &lt;Purpose text="员工司机" value="Associate Driver"/&gt;
  &lt;Purpose text="审核员" value="Auditors"/&gt;
  &lt;Purpose text="新员工" value="New Joinee"/&gt; 
  &lt;Purpose text="其他" value="Others"/&gt; 
&lt;/PurposeList&gt;</value>
    <comment>Purpose</comment>
  </data>
  <data name="TempCardGenerated" xml:space="preserve">
    <value>临时卡已制作好</value>
    <comment>Temporary ID Card is generated Successfully</comment>
  </data>
  <data name="Welcome" xml:space="preserve">
    <value>欢迎</value>
    <comment>Welcome</comment>
  </data>
  <data name="DayInAdvance" xml:space="preserve">
    <value>天内的申请</value>
    <comment>days in advance.</comment>
  </data>
  <data name="lblTodateMandatory" xml:space="preserve">
    <value>请选择有效的日期</value>
    <comment>Please select a valid Date in the ToDate field</comment>
  </data>
  <data name="RequestallowedCheck" xml:space="preserve">
    <value>只允许</value>
    <comment>Requests are allowed to raise for only</comment>
  </data>
  <data name="Issued" xml:space="preserve">
    <value>使用中</value>
    <comment>Issued</comment>
  </data>
  <data name="LocationCard" xml:space="preserve">
    <value>办公地点还卡成功</value>
  </data>
  <data name="Returned" xml:space="preserve">
    <value>已还卡</value>
    <comment>Returned</comment>
  </data>
  <data name="CurrentTimeError" xml:space="preserve">
    <value>开始时间不能在系统当前时间之前</value>
    <comment>From time should be greater than or equal to current time</comment>
  </data>
  <data name="EmergencyContactTime" xml:space="preserve">
    <value>紧急联系的结束时间必须在开始时间之后</value>
    <comment>Emergency Contact From Date and To Date should not be lesser than Current Date</comment>
  </data>
  <data name="VisitDurationError" xml:space="preserve">
    <value>访问期限不能超过</value>
    <comment>Visit duration should be less than or equal to </comment>
  </data>
  <data name="VisitDurationErrorday" xml:space="preserve">
    <value>天</value>
    <comment>days</comment>
  </data>
  <data name="VisitTimeMandatory" xml:space="preserve">
    <value>访问开始日期和结束日期不能在系统当前日期之前</value>
    <comment>Visit From Date and Visit To Date should be greater than or equal to current date</comment>
  </data>
  <data name="BadgeFacility" xml:space="preserve">
    <value>设施名称 :</value>
  </data>
  <data name="CurrentTimeFromTimeToTime" xml:space="preserve">
    <value>结束时间必须在开始时间之后</value>
    <comment>To Time should be greater than From Time</comment>
  </data>
  <data name="Ok" xml:space="preserve">
    <value>确认</value>
    <comment>Ok</comment>
  </data>
  <data name="FaciltyCardReturn" xml:space="preserve">
    <value>还卡成功</value>
  </data>
  <data name="FromDateValidation" xml:space="preserve">
    <value>开始日期不能在结束日期之后</value>
  </data>
  <data name="VisitFromTimeMandatory" xml:space="preserve">
    <value>访问开始日期不能在系统当前日期之前</value>
  </data>
  <data name="VisitToTimeMandatory" xml:space="preserve">
    <value>访问结束日期不能在系统当前日期之前</value>
  </data>
  <data name="CityIVS" xml:space="preserve">
    <value>城市 :</value>
  </data>
  <data name="FacilityIVS" xml:space="preserve">
    <value>设施名称 :</value>
  </data>
  <data name="lblEquipmentCust" xml:space="preserve">
    <value>Token 号码 :</value>
    <comment>Token Number</comment>
  </data>
  <data name="lblEquipmentsCustody" xml:space="preserve">
    <value>保管物品</value>
    <comment>Equipments In Custody</comment>
  </data>
  <data name="lbltokenNumber" xml:space="preserve">
    <value>Token 号码 :</value>
    <comment>Token Number</comment>
  </data>
  <data name="Description" xml:space="preserve">
    <value>描述</value>
    <comment>Description</comment>
  </data>
  <data name="Disclaimer" xml:space="preserve">
    <value>Disclaimer : By returning this token I hereby confirm that I have received the asset</value>
    <comment>Disclaimer : By returning this token I hereby confirm that I have received the asset</comment>
  </data>
  <data name="DisclaimerNext" xml:space="preserve">
    <value>described here.</value>
    <comment>described here.</comment>
  </data>
  <data name="TerminatedMessage" xml:space="preserve">
    <value>离职员工</value>
    <comment>This associate is no longer with Cognizant.</comment>
  </data>
</root>
