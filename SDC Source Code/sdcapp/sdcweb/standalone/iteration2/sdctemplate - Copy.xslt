<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0" xmlns:sr="http://www.cap.org/pert/2009/01/">
	
  <xsl:output encoding="us-ascii" method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
	
  <!--<xsl:variable name="form-action" select="document('sr-service.xml')/sr:sr-service"/>-->
	<xsl:variable name="show-toc" select="'false'"/>
	<!--<xsl:variable name="sct-code" select="document('sct-codes.xml')/sr:concept-descriptors"/>-->
	<xsl:variable name="template-links" select="document('sr-toc.xml')/sr:template-toc"/>
	<xsl:variable name="debug" select="'false'"/>
	
	<xsl:template match="/">
		
		<xsl:variable name ="required" select="string(//Header/Property[@type='web_posting_date meta']/@val)"/>
        <html>
            <head>
			<!--<link rel="stylesheet" href="sdctemplate.css" type="text/css" />-->
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
			<!--<script type="text/javascript" src="sdc.js"></script>-->
            
			<style>
				Body 
				{
					font-family: Arial;
					font-size: 12px;
					width: 100%;
					margin-left: auto;
					margin-right: auto;
				}

				li
				{
					padding: 1px;
					color: blue;
					list-style-type: square;
				}

				p
				{
					margin-top:2px;
					margin-bottom:2px;
					padding:0px;
				}
				.BodyGroup
				{
					width: 900px;
					margin-left: auto;
					margin-right: auto;
					text-align: left;
				}

				.HeaderGroup
				{
					width: 99%;
					margin-top: 5px;
					margin-bottom: 5px;
					margin-left: auto;
					margin-right: auto;
					border: solid 1px #000080;
					padding: 5px;
					clear: both;
				}

				.HeaderGroupText
				{
					font-size: 12px;
					font-weight: bold;
					background-color: #FFFFFF;
					color: #000080;
					padding: 8px;
					vertical-align: bottom;
				}

				.HeaderInfoText
				{
					font-size: 12px;
					text-align: right;
					font-style: italic;    
				}

				.HeaderText
				{
					text-align: center;
					font-size: 18px;
					font-weight: bold;
					background-color: #FFFFFF;
					color: #000080;
					padding: 8px;
					vertical-align: bottom;
				}

				.TopHeader
				{
					text-align: center;
					font-size: 30px;
					font-weight: bold;
					background-color: #000080;
					color: #FFFFFF;
					padding: 2px;
				}


				.subSection
				{
					text-align: center;
					font-size: 20px;
					font-weight: bold;
					background-color: #000080;
					color: #FFFFFF;
					padding: 2px;
				}

				.thinBox
				{
					text-align: center;
					font-size: 14px;
					font-weight: bold;
					font-style:italic;
					background-color: #000080;
					color: #FFFFFF;
					padding: 2px;
				}


				.QuestionReset
				{
					float: right;
					padding-left: 2px;
					font-size: 10px;
					text-decoration: none;
					font-style: normal;
					color: Blue;
				}


				.QuestionInSection
				{
					float: left;
					width: 49%;
					padding: 2px;
				}

				.QuestionInQuestion
				{
					float: left;    
					padding: 2px;
				}

				.QuestionInListItem
				{
					padding: 2px;
					padding-left: 20px;
				}



				.QuestionTitle
				{
					padding: 2px;
					background-color: #E9E9E9;
					font-size: 13px;
					font-style: italic;
					font-weight: bold;
					clear: both;
				}

				.Answer
				{
					float: left;
					padding-top: 2px;
					width: 93%;
				}

				.DisplayProperty
				{
					float: left;
					padding-top: 2px;
					width: 22%;
				}

				.DisplayProperty1
				{
					float: left;
					padding-top: 2px;
					padding-left: 5px;
				}

				.AnswerTextBox
				{
					height: 11px;
					font-size: 10px;
					padding-left: 2px;
					margin-left: 4px;
				}

				.NoteText
				{
					padding: 2px;
					font-weight: bold;
					color: Blue;
					clear: both;
				}

				.TextBox
				{
					padding: 2px;
					width: 98%;
					height: 15px;
				}

				.SubmitButton
				{
					padding: 2px;
					text-align: center;
				}

				.MessageDataResult
				{
					padding-bottom: 20px;
					padding-top: 20px;
					clear: both;
					padding-left: 10px;
					padding-right: 10px;
				}

				.MessageDataQuestion
				{
					padding-left: 20px;
					color: blue;
					font-style: italic;
				}

				.MessageDataAnswer
				{
					padding-left: 20px;
					color: green;
					font-style: normal;
					font-weight: bold;
				}

				.MessageDataChecklist
				{
					font-weight: bold;
					color: red;
				}


				.header_text
				{
					border-bottom:1px solid black;padding-bottom:0px;text-align:left;
				}

				.title
				{
					text-align: center;
					font-size: 18px;
					font-weight: bold;
					font-style:italic;
					background-color: #FFFFFF;
					color: #000080;
					padding: 8px;
					vertical-align: bottom;

				}

				#S1
				{
					font-size: 18px;
					font-weight: bold;
					background-color: #FFFFFF;
					color: #000080;
					padding: 8px;
					vertical-align: bottom;
				}
				.right.noBreak{float: right}
				.right{float: right;clear:both}
				.float-right{float: right;}
				.Header_left
				{
					color:purple;
					text-align: left;
					font-size: 18px;
					font-weight: bold;
					
				}
				.left
				{
					text-align:left;clear:both;
				}
				.copyright
				{
					font-style:italic;
					float:left;
				}
			</style>
			
				<script type="text/javascript">
				
					<xsl:text disable-output-escaping="yes" >
						<![CDATA[
							var xmlDoc;

							$(document).ready(function () {

								jQuery.support.cors = true;  //not sure if needed because cors setting is on the server
								
								/*
									save original xml in jquery variable  
									server or xslt puts original xml in #rawxml, issue with xslt putting in xml is copy-of function decodes special characters, thus 
									making xml invalid
								*/
								var xmlstring = $("#rawxml").val();   

								//remove declaration part if exists
								xmlstring = xmlstring.substring(xmlstring.indexOf("<FormDesign"));
								$("#rawxml").val(xmlstring);
								
								//load into xml dom
								try{
									xmlDoc = $.parseXML(xmlstring);
									$xml = $(xmlDoc);
								}
								catch(err){
									alert(err.message);
								}

							});


							function sayHello(name) {
								alert("sayHello function in javascripts says - hello, " + name);
								return window.external.ShowMessage("If you can see this message sayHello successfully called ShowMessage function in this desktop client app.");


							}

							var repeat = 1;
							//adds a new repeat of a section
							function addSection(obj) {
								//obj is btnAdd

								//we need to clone table, so get table
								var td = obj.parentElement;
								var table = td.parentElement  //tr
											   .parentElement  //tbody
											   .parentElement //table
								
								var sectionGuid = table.parentElement.id;

								var max = table.parentElement.firstChild.value;  //maxcardinality
								
								var textboxes = td.getElementsByTagName("input");  //get hidden input, radio buttons, checkboxes and input text boxes


								

								var newtable = table.cloneNode(true);    

								newtable.id = table.id;   
								repeat = +repeat + 1;


								//make the remove button visible 
								//and hide the add button on the previous section
								obj.nextSibling.style.visibility = "visible";
								obj.style.visibility = "hidden";    

								var trace = 0;
								var newname;
								var i;
								var parentGuid;
								var ID;

								//add the new repeat
								try {
									parentsection = table.parentElement;
									//append newtable after setting properties of individual elements
									//parentsection.appendChild(newtable);  //add new table - note that table.id is the same for new table
									

									//find parentGuid
									for (i = 0; i < textboxes.length; i++) {
										if (textboxes[i].type == "hidden") {
											var components = textboxes[i].name.split(":");
											parentId = components[1];

											//remove repeat indicator if exists from Guid
											if (parentId.indexOf(",") > 0)
												parentId = parentId.substring(0, parentId.indexOf(","));
											break;
										}
										
									}

									/*add a new section node in xml by cloning the section with instanceGuid = parentGuid
									since there will be repeat sections with the same instanceGuid, take the last instance
									and clone it*/

									var $sectionCurrent = $xml.find('Section[ID="' + parentId.substring(1) + '"]:last');
										   
									sectionid = $sectionCurrent.attr('ID');
									var $sectionNew = $sectionCurrent.clone(true);

									//iterate through textboxes and assign new unique ids to them
									for (i = 0; i < textboxes.length; i++) {

										if (textboxes[i].type == "hidden" || textboxes[i].type == "text" || textboxes[i].type=="radio") {

											oldname = textboxes[i].name;

											if(textboxes[i].id=="maxcardinality")
												  continue;

											if(textboxes[i].name=="")
											{
												alert("error: a " + textboxes[i].type + " box without name is found at " + i);
												continue;
											}

											var components = oldname.split(":");
											var newguid = generateGuid();

											//if (i == 0) testguid = newguid;

											parentId = components[1];

											//remove repeat indicator if exists from Guid
											if (parentId.indexOf(",") > 0)
												parentId = parentGuid.substring(0, parentId.indexOf(","));

											ID = components[0].substring(1);

										   
											if (textboxes[i].type == "hidden") {
											   //note that parent has repeat indicator
												newname = "q" + ID + '-' + newguid + ":" + parentId;
											}

											
											//find the element in the new table
											var item;
											var allelements = newtable.getElementsByTagName('*');
											for(k=0;k<allelements.length;k++)
											{
											   if(allelements[k].name == textboxes[i].name)
											   {
												   item = allelements[k];

												   break;
											   }
											 }
											
											

											if (item != null) {

																	
												if(item.type=="hidden")   //question will have Q as the first letter
												{                                             //find question in xml fragment
												   $question = $sectionNew.find('Question[ID="' + ID + '"]');
												   $question.attr("ID", ID + '-' + newguid);                       
												   item.name = newname;
												  
												}
												else {                   //answers do not have Q
														item.name = newname.substring(1);
														if(item.type=="radio" || item.type == "checkbox")
															 item.checked = false;
														 else
															 item.value = "";
												}

											}

										}
									}

									//better to append new table after setting properties of individual controls
									parentsection.appendChild(newtable);

									//insert newsec after section
									$sectionCurrent.after($sectionNew);


									//hide the Add button if max count reached
									if (countRepeats(sectionGuid) == max) {
										// alert('max repeat = ' + max + ' reached');
										inputs = newtable.getElementsByTagName('*');
										for(m=0;m<inputs.length;m++)
										{
									if(inputs[m].id=="btnAdd")
											   inputs[m].style.visibility="hidden";
											if(inputs[m].id=="btnRemove")
											   inputs[m].style.visibility="visible";
										}

									}



								}
								catch (err) {
									alert(err.message + "\n" + trace + "\n" + newname + "n" + i);
								}

							}


							function generateGuid() {
								var result, i, j;
								result = '';
								for (j = 0; j < 32; j++) {
									if (j == 8 || j == 12 || j == 16 || j == 20)
										result = result + '-';
									i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
									result = result + i;
								}
								return result;
							}


							function countRepeats(sectionid) {

								var section = document.getElementById(sectionid);  //
								var tables = section.getElementsByTagName('TABLE');
								var count = 0;
								for(i=0; i<tables.length; i++)
								{
								   if(tables[i].id == sectionid) count++;
								}
							   
								return count;

							}

							function getLastRepeat(sectionid) {
								var section = document.getElementById(sectionid);
								var tables = section.parentElement.getElementsByTagName('TABLE');
								var ret = null;
								for(i=0;i<tables.length;i++)
								{
								   if(tables[i].id==sectionid)
									 ret = tables[i];
								}
								return ret;

							}

							function removeSection(obj) {
								td = obj.parentElement;
								tr = td.parentElement;
								tbody = tr.parentElement;
								table = tbody.parentElement;
								var section = table.parentElement;
								section.removeChild(table);

								//make Add button on the last repeat visible again
								last = getLastRepeat(section.id);
								if (last==null)
								{
									alert("Error: last repeat is null");
									return false;
								}
								inputs = last.getElementsByTagName('*');
								for(m=0;m<inputs.length;m++)
								{
								if(inputs[m].id=="btnAdd")
									{
										 inputs[m].style.visibility="visible";
									}
								}
								
								
								id = section.id;
								//repeat = table.id;
								$xml.find('Section[ID="' + id + '"]').eq(table.id).remove(); 
							}


							/*
							Helper functions
							*/
							function trim(input) {
							input = input.replace(/^\s+|\s+$/g, '');
							return input;
							}

							function findElementById(parentId, Id) {
							   //finds an element among descedants of a given node
							   var parent = document.getElementById(parentId);

							   var children = parent.getElementsByTagName('*');


							   for (i = 0; i < children.length; i++) {

								  if (children[i].id == Id) {
									 return children[i];
								  }
							   }

							}

							function findElementByName(parentName, Name) {
							  //finds an element among descedants of a given node
							  var parent = document.getElementById(parentName);
							  var children = parent.getElementsByTagName('*');
							  
							  for (i = 0; i < children.length; i++) {
								 if (children[i].name == Name) {
									 return children[i];
								 }
							  }
							}

							function xmlToString(xmlData) {

								var xmlString;
								//no longer needed for newer IE
								//IE
								//if (window.ActiveXObject) {
								//    xmlString = xmlData.xml;
								//}
								// code for Mozilla, Firefox, Opera, etc.
								//else {
								xmlString = (new XMLSerializer()).serializeToString(xmlData);
								//}
								return xmlString;
							}

							//helper functions end

							//submit form calls this function
							/*
							Builds flatXml, updates the original xml with answers.
							Note that new section nodes for repeat sections have already been added (upon clicking btnAdd - addSection function) 
							*/
							var flatXml;
							function openMessageData() {

								var sb = "";
								var q, answer = "";
								var elem = document.getElementById("checklist").elements;
								var response = "<response>";
								var html = response;

								for (var i = 0; i < elem.length; i++) {
									q = "";
									var name = elem[i].name;

									var value;

									//split up name into instanceGuid and parentGuid and question id
									//var components = name.split(':');
									//var instanceGuid = components[0].substring(1);
									//var parentGuid = components[1];
									//var id = components[2];
									var instanceGuid = '';
									var parentGuid = '';
									var id = name;
									if (name.indexOf("q") == 0) {
										value = elem[i].value;

										answer = GetAnswer(name.substring(1));

										if (answer != "") {

											response += "<question ID=\"" + id + "\" display-name=\"" + value.replace(/</g, "&lt;").replace(/>/g, "&gt;") 
													 + "\">";
											response += answer + "</question>";               

											q += "<div class=\"MessageDataQuestion\">&lt;question ID=\"" + id + "\" display-name=\"" + value + "";
											q += "&gt;<br><div class=\"MessageDataAnswer\">" + answer.replace(/</g,"&lt;").replace(/>/g,"&gt;") + "</div>&lt;/question&gt;</div>";


										}
										sb += q;
										answer = "";
									}
								}

								response = response.replace(/<br>/g, "");
								response = response + "</response>";

								flatXml = response;


								sb = "<div style='font-weight:bold; color:purple'>Flat Xml response</div>" 
									 + "<div class=\"MessageDataChecklist\">&lt;response&gt;" + sb + "&lt;/response&gt;</div>"
									 + "<br/><div style='font-weight:bold; color:purple'>Response xml sent to web service.</div>"


								document.getElementById('MessageDataResult').innerHTML = sb;
								document.getElementById('MessageData').style.display = 'block';
								document.getElementById('FormData').style.display = 'none';
								
								
								//update Xml with answers
								updateXml();
								
								

								//var test = xmlToString(xmlDoc);
								//document.getElementById('rawxml').innerText = test;

								//Ajax call to web service
								CallTestService(xmlToString(xmlDoc));
								
							}

							function closeMessageData() {
								document.getElementById('MessageData').style.display = 'none';
								document.getElementById('response').style.display = 'none';
								document.getElementById('FormData').style.display = 'block';
							}

							function GetAnswer(qCkey) {
								var elem = document.getElementById("checklist").elements;
								var str = "";
								var name, value;

								for (var i = 0; i < elem.length; i++) {
									name = elem[i].name;
									value = elem[i].value;

									if (name.indexOf(qCkey) == 0) {

										if (elem[i].checked || (elem[i].type == "text" && value != "")) {

											{

												var k = value.split(',');

												if (elem[i].type == "text" && value != "") {
													//str += "&lt;answer value=\"" + value + "\"/&gt;<br>";
													str += "<answer value=\"" + value + "\"/><br>";
												}
												else if (elem[i].type != "text") {
													//str += "&lt;answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/&gt;<br>";
													str += "<answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/><br>";
												}
											}
										}
									}
								}
								return str;
							}

							function GetDisplayName(value) {
								var strArray = value.split(',');
								var returnStr = "";
								if (strArray.length > 1) {
									for (var i = 1; i < strArray.length; i++) {
										if (i != strArray.length) {
											returnStr += strArray[i] + ",";
										}
										else {
											returnStr += strArray[i];
										}
									}
								}
								returnStr = returnStr.replace(/</g,"&lt;").replace(/>/g,"&gt;");
								return returnStr.substr(0, returnStr.length - 1);
							}


							//updates answers in full xml
							function updateXml() {
								var $xml = $(xmlDoc);  //full xml
								FlatDoc = $.parseXML(flatXml);
								$xmlFlatDoc = $(FlatDoc);
								$xmlFlatDoc.find('question').each(function () {
									var $question = $(this);
									var questionid = $question.attr("ID");

									if(questionid.indexOf(":")>0)
										questionid = questionid.substring(0,questionid.indexOf(':'));

									//if(questionid.indexOf("-")>0)
									//    questionid = questionid.substring(0,questionid.indexOf('-'));

									questionid = questionid.substring(1);

									var repeat = 0;
									


									//there may be multiple answers per question
									$question.find('answer').each(function () {
										var $test = $(this);
										var id = $test.attr("ID");
										var val = $test.attr("value");

										
										var $targetQuestion = $(xmlDoc).find("Question[ID='" + questionid + "']");
										var targetQuestionId = $targetQuestion.attr("ID");


										if (id != null) {
											 
											var $targetAnswer = $targetQuestion.find("ListItem[ID='" + id + "']");
											$targetAnswer.attr("selected", "true");
											//alert("set selected to true");
											if ($targetAnswer.find("ListItemResponseField") != null) {
												val = $question.find('answer').next().attr("value");
												$response = $targetAnswer.find("Response").children(0);
												$response.attr("val", val);
											}

										}
										else {  //free response
											
											$targetAnswer = $targetQuestion.find("ResponseField").find("Response");
											$targetAnswer.children(0).attr("val", val);
										}
									});

								});
								
							   

							}

							/*
							function CallTestService(data) {
								var webServiceURL = "http://www.sharedapps.net/SDCReceiver/service1.asmx";
								webServiceURL = prompt("Enter endpoint:", webServiceURL);
								
								$.support.cors = true;
								var xmldata = encodeURIComponent(data);
								
								var soapRequest =
												'<?xml version="1.0" encoding="utf-8"?>' +
													' <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"' +
															' xmlns:xsd="http://www.w3.org/2001/XMLSchema"' +
															' xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">' +
														' <soap:Body>' +
																' <SubmitFormRequest xmlns="urn:ihe:iti:rfd:2007">' +
																' <body>' + xmldata + '</body> ' +
																' </SubmitFormRequest>' +
														' </soap:Body>' +
													' </soap:Envelope>';

								
								soapAction = "urn:SubmitForm";
								

								$.ajax({
									type: "POST",
									url: webServiceURL,
									contentType: "text/xml",
									dataType: "xml",
									processData: false,
									headers: {
										"SOAPAction": soapAction
									},
									data: soapRequest,
									success: OnSuccess,
									error: OnError
								});

								return false;
							}
							*/
							
							function CallTestService(data) {
								var webServiceURL = "https://sdcpilot.azurewebsites.net/SDCService.asmx";
								webServiceURL = prompt("Enter endpoint:", webServiceURL);
								
								$.support.cors = true;
								var xmldata = encodeURIComponent(data);
								
								var soapRequest =
												'<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">' + 
												 '  <soapenv:Header/>' +
												  ' <soapenv:Body>' + 
													 ' <tem:SubmitFormRequest>' + data + '</tem:SubmitFormRequest>' +
												  ' </soapenv:Body>' + 
												' </soapenv:Envelope>'

								
								soapAction = "http://tempuri.org/SubmitFormRequest";
								
								alert(soapRequest);
								$.ajax({
									type: "POST",
									url: webServiceURL,
									contentType: "text/xml",
									dataType: "xml",
									processData: false,
									headers: {
										"SOAPAction": soapAction
									},
									data: soapRequest,
									success: OnSuccess,
									error: OnError
								});

								return false;
							}

							function OnSuccess(data, status) {
								alert("Response received from the receiver.");
								var xmlstring = xmlToString(data);
							    
								if (document.getElementById("response") != null) {       
									document.getElementById("response").textContent ="Receiver Response - " + xmlstring;
									$("#response").css("background-color", "yellow");
									$("#response").css("display", "block");
								}
								
								
							}

							function OnError(xhr, textStatus, err) {
								alert('Status:' + xhr.status + ', Text:' + textStatus + ', Error: ' + err);
							}


						]]>
					</xsl:text>
					
				</script>
				
			
			 
            </head>
            <body align="left">
				<xsl:if test="$show-toc='true' and count($template-links/template-link) &gt; 0">
					<div style="width:250px;height:600px;float:left;margin:10px;padding-left:10px;padding-right:20px">
						<xsl:apply-templates select="$template-links"/>
						<br/>
						<p/>
					</div>
				</xsl:if>
            	<!--hidden textbox to store rawxml at run time-->
            	<!--<input type="hidden" id = "rawxml"/>-->
            	<textarea style="width:90%;margin:40px;background-color:lightyellow" id = 'rawxml'> 
		               <xsl:copy-of select="node()"/>
		               <!--<xsl:copy-of select="document('Breast Invasive M3.xml')"/>-->
         		</textarea>	
				<div class="BodyGroup">
					<xsl:if test="$show-toc='true' and count($template-links/template-link) &gt; 0">
						<xsl:attribute name="style">
							<xsl:text>float:left</xsl:text>
						</xsl:attribute>
					</xsl:if>
					
					<div id="MessageData" style="display:none;">
						<table class="HeaderGroup" align="center">
							<tr>
								<td>
									<div class="TopHeader">
										Structured Report Data
									</div>
									<div id="MessageDataResult" class="MessageDataResult"/>
									
									
									<div class="SubmitButton">
										<input type="button" value="Back" onClick="javascript:closeMessageData()" />
									</div>
								</td>
							</tr>
						</table>
					</div>
					
					<div id="response" style="display:none;">
					</div>
					
					<div id="FormData">
						<form id="checklist" name="checklist" method="post" >
							<xsl:attribute name="action">
								<!--<xsl:value-of select="$form-action"/>-->
							</xsl:attribute>
							
							<!--show header-->
							<xsl:variable name="title_style" select="//Header/@styleClass"/>
							<xsl:variable name='title_id' select="//Header/@ID"/>
							<div ID = '{$title_id}' class="Header_{$title_style}">								
								<xsl:value-of select="//Header/@title"/>
							</div>
							
							<div>
							<xsl:for-each select="//Header/Property">
								<xsl:variable name="textstyle" select="@styleClass"/>
									<p class='{$textstyle}'><xsl:value-of select="@val"/></p>
								<!--
									
									<xsl:choose>
										<xsl:when test="@type='AJCC_UICC_Version meta'">
											AJCC UICC Version:
										</xsl:when>
										<xsl:when test="@type='CS'">
											CS Version:
										</xsl:when>
										<xsl:when test="@type='web_posting_date meta'">
											Protocol web posting date:
										</xsl:when>
									</xsl:choose>
									
									<xsl:if test="@type!='approval-status meta'">
										<xsl:value-of select="@val"/>
									</xsl:if>
									-->
								<!--<div style="clear:both"/>-->
								
							</xsl:for-each>
								
							</div>
							
							<!--show body-->
							<xsl:apply-templates select="//Body/ChildItems/Section" >
								<xsl:with-param name="required" select="$required" />
								<xsl:with-param name="parentId" select="'*'"/> <!--parentId = * for outermost --> 
							</xsl:apply-templates>
							<xsl:apply-templates select="//Body/ChildItems/Question" mode="level2" >
								<xsl:with-param name="required" select="$required" />
								<xsl:with-param name="parentId" select="'*'"/>  <!--parentId = * for outermost --> 
							</xsl:apply-templates>
							
							<!--<xsl:if test="contains($form-action, 'http') or contains($form-action, 'javascript')">-->
								<!--remove submit button for the desktop verion-->
								
								<div class="SubmitButton">
									<input type="submit" value="Submit" onclick="javascript:openMessageData();return false;"/>
								</div>
							<!--</xsl:if>-->
						</form>
					</div>
				</div>
            </body>
        </html>
    </xsl:template>
   
    <xsl:template match="//Header">
       
       
    </xsl:template>
	
	<xsl:template match="Section">
		<xsl:param name="parentSectionId"/>		
		<xsl:if test="not (@visible) or (@visible='true')">
			<xsl:variable name="required" select="true"/>
			<xsl:variable name="style" select="@styleClass"/>
			<xsl:variable name="defaultStyle" select="'TopHeader'"/>
			<xsl:variable name="sectionId" select="concat('s',@ID)"/>
			<div> 
			        <xsl:attribute name="id">
					<xsl:value-of select="concat($sectionId,':',parentSectionId,':','1')"/>
					
				</xsl:attribute>
				<input id = "maxcardinality" type="hidden">
					<xsl:attribute name="value">
						<xsl:value-of select="@maxCard"/>
					</xsl:attribute>
					
				</input>
				
				<!-- table is repeated if cardinality is greater than 1 and id value will be incremented-->
				<table class="HeaderGroup" align="center">
				   
				   <xsl:attribute name="id">
				   	<!--<xsl:value-of select="1"/>-->
				   	<xsl:value-of select="concat($sectionId,':',parentSectionId,':','1')"/>
				   </xsl:attribute>
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="$style!=''">
									<div class="{$style}">										
										<xsl:value-of select="@title"/>
									</div>
								</xsl:when>
								<xsl:otherwise>
									<div class="{$defaultStyle}">
										<xsl:value-of select="@title"/>
									</div>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:choose>
								<xsl:when test="$required='false'">

								</xsl:when>
								<xsl:otherwise>	
									<xsl:apply-templates select="ChildItems/Question" mode="level1" >
										<xsl:with-param name="required" select="'true'"/>
										<xsl:with-param name="parentSectionId" select="$sectionId"/>
									</xsl:apply-templates>

									<xsl:apply-templates select="ChildItems/Section" >
										<xsl:with-param name="required" select="'true'"/>
										<xsl:with-param name="parenSectiontId" select="$sectionId"/>
									</xsl:apply-templates>

								</xsl:otherwise>
							</xsl:choose>
							<div style="clear:both"/>
							<xsl:if test="@maxCard&gt;1">
								<input type="button" id="btnAdd" onclick="addSection(this)" value="+"/>
								<input type="button" id ="btnRemove" onclick="removeSection(this)" value="-">
									<xsl:attribute name = "style">
										<xsl:value-of select="'visibility:hidden;'"/>
									</xsl:attribute>
								</input>
							</xsl:if>
						</td>
					</tr>
				</table>
			</div>
		
		</xsl:if>
	</xsl:template>
	
	<!--question in section-->
	<xsl:template match="Question" mode="level1">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID,':',$parentSectionId)"/>		            
    		<input type="hidden" class="TextBox">
				<xsl:attribute name="name">
					<xsl:value-of select="$questionId"/>
				</xsl:attribute>				
				<xsl:attribute name="value">
					<xsl:value-of select="@title"/>
				</xsl:attribute>
    		</input>
    
		<div class="QuestionInSection">   <!--two columns-->
			<div class="QuestionTitle">
				<xsl:value-of select="@title"/> 
				<xsl:if test="ResponseField"> 
					<input type="text" class="TextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="ResponseField/Response//@val"/>
							<!--<xsl:value-of select="substring($expandedId,2)"/>-->
						</xsl:attribute>
					</input>
				</xsl:if>
			</div>
			
			<div style="clear:both;"/>
			
			<xsl:if test="ListField">
			  <xsl:apply-templates select="ListField" mode="level1">
				  <xsl:with-param name="questionId" select="$questionId" />
				  <xsl:with-param name="parentSectionId" select="$parentSectionId" />
			  </xsl:apply-templates>
			</xsl:if>
			
			<!--11/13/2016: question within question-->
			<div style="clear:both;"/>
			<xsl:if test="ChildItems/Question">				
				<xsl:apply-templates select="ChildItems/Question" mode="level3">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
			</div>
		
	</xsl:template>

	
	<!--question in list item-->
<xsl:template match="Question" mode="level2">
	<xsl:param name="parentSectionId"/>
	<xsl:variable name="questionId" select="concat('q',@ID,':',$parentSectionId)"/>

    <input type="hidden" class="TextBox">
      <xsl:attribute name="name">
        <xsl:value-of select="$questionId"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="@title"/>
      </xsl:attribute>
    </input>
    
	<div class="QuestionInListItem"> 	  
		<xsl:choose>
			<!--not showing the hidden question-->
			<xsl:when test="string-length(@title)&gt;0">
				<div class="QuestionTitle">
					<xsl:value-of select="@title"/> 
					<xsl:if test="ResponseField">
						<input type="text" class="TextBox">
							<xsl:attribute name="name">
								<xsl:value-of select="substring($questionId,2)"/> <!--drop q-->
							</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="ResponseField/Response//@val"/>
							</xsl:attribute>
						</input>
					</xsl:if>
				</div>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="ResponseField">
					<input type="text" class="TextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="ResponseField/Response//@val"/>
						</xsl:attribute>
					</input>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
		
		<div style="clear:both;"/>
		<xsl:if test="ListField">
    		<xsl:apply-templates select="ListField" mode="level1">
				<xsl:with-param name="questionId" select="$questionId" />
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
        	</xsl:apply-templates>
		</xsl:if>
		<!--11/13/2016: question within question-->
		<!--<xsl:if test="ChildItems/Question">
			
			<xsl:apply-templates select="ChildItems/Question" mode="level2">
				<xsl:with-param name="parentSectionId" select="$parentSectionId" />
			</xsl:apply-templates>
		</xsl:if>-->
		
	</div>

</xsl:template>

	<!--question in question-->
	<xsl:template match="Question" mode="level3">
		<xsl:param name="parentSectionId"/>
		<xsl:variable name="questionId" select="concat('q',@ID,':',$parentSectionId)"/>		            
		<input type="hidden" class="TextBox">
			<xsl:attribute name="name">
				<xsl:value-of select="$questionId"/>
			</xsl:attribute>				
			<xsl:attribute name="value">
				<xsl:value-of select="@title"/>
			</xsl:attribute>
		</input>
		
		<div class="QuestionInQuestion">   
			<div class="QuestionTitle">
				<xsl:value-of select="@title"/> 
				<xsl:if test="ResponseField"> 
					<input type="text" class="TextBox">
						<xsl:attribute name="name">
							<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="ResponseField/Response//@val"/>
							<!--<xsl:value-of select="substring($expandedId,2)"/>-->
						</xsl:attribute>
					</input>
				</xsl:if>
			</div>
			
			<div style="clear:both;"/>
			
			<xsl:if test="ListField">
				<xsl:apply-templates select="ListField" mode="level1">
					<xsl:with-param name="questionId" select="$questionId" />
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
			<!--11/13/2016: question within question-->
			<div style="clear:both;"/>
			<xsl:if test="ChildItems/Question">				
				<xsl:apply-templates select="ChildItems/Question" mode="level3">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:if>
			
		</div>
		
	</xsl:template>

<xsl:template match="ListField" mode="level1">
   <xsl:param name="questionId" />
   <xsl:param name="parentSectionId" />  
	<xsl:choose>
		<!--multiselect-->
		<xsl:when test="@multiSelect='true'">
			<xsl:for-each select="List/ListItem">
				<div class="Answer">
					<input type="checkbox" style="float:left;">
						<xsl:attribute name="name">
              				<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:attribute name="value">
							<xsl:value-of select="@ID"/>,<xsl:value-of select="@title"/>
						</xsl:attribute>
						<xsl:if test="@selected='true'">
							<xsl:attribute name="checked">
							</xsl:attribute>
						</xsl:if>
					</input>
					<xsl:value-of select="@title"/>
					
					<xsl:if test="ListItemResponseField">
						<input type="text" class="AnswerTextBox">
						    <xsl:attribute name="name">
								<xsl:value-of select="substring($questionId,2)"/>
						    </xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="ListItemResponseField/Response//@val"/>
							</xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<!--question within list-->
				<xsl:apply-templates select="Question" mode="level2">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>
			</xsl:for-each>
		</xsl:when>
	
		<!--single select-->
		<xsl:otherwise>
			<xsl:for-each select="List/ListItem">
				<div class="Answer">
					<input type="radio" style="float:left">
						<xsl:attribute name="name">
              				<xsl:value-of select="substring($questionId,2)"/>
						</xsl:attribute>
						<xsl:if test="@selected='true'">
							<xsl:attribute name="checked">
							</xsl:attribute>
						</xsl:if>
						
						<xsl:attribute name="value">
							<xsl:value-of select="@ID"/>,<xsl:value-of select="@title"/>
						</xsl:attribute>
					</input>
					<xsl:value-of select="@title"/>
					<!--answer fillin-->
					<xsl:if test="ListItemResponseField">
						<input type="text" class="AnswerTextBox">
							<xsl:attribute name="width">
								<xsl:value-of select="100"/>
							</xsl:attribute>
							<xsl:attribute name="name">
								<xsl:value-of select="substring($questionId,2)"/>
							</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="ListItemResponseField/Response//@val"/>
							</xsl:attribute>
						</input>
					</xsl:if>
				</div>
				<!--question within list-->
				<xsl:apply-templates select="ChildItems/Question" mode="level2">
					<xsl:with-param name="parentSectionId" select="$parentSectionId" />
				</xsl:apply-templates>	
			</xsl:for-each>
		</xsl:otherwise>
		
	</xsl:choose>

</xsl:template>

</xsl:stylesheet>