<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0" xmlns:sr="http://www.cap.org/pert/2009/01/">
	
  <xsl:output method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
	
  <xsl:variable name="form-action" select="document('sr-service.xml')/sr:sr-service"/>
	<xsl:variable name="show-toc" select="'false'"/>
	<xsl:variable name="sct-code" select="document('sct-codes.xml')/sr:concept-descriptors"/>
	<xsl:variable name="template-links" select="document('sr-toc.xml')/sr:template-toc"/>
	<xsl:variable name="debug" select="'false'"/>
	
	<xsl:template match="/">
		
		<xsl:variable name ="required" select="string(//Header/Property[@type='web_posting_date meta']/@val)"/>
        <html>
            <head>
			<![CDATA[
   
]]>
            <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
			<!--<script type="text/javascript" src="sdc.js"></script>-->
            
			 <link rel="stylesheet" href="sdctemplate.css" type="text/css" />
			 
			 <script type="text/javascript">
			 	<!---start -->

			 	var xmlDoc;

				$(document).ready(function () {

				    jQuery.support.cors = true;
				    //save original xml in jquery variable  
				    var xmlstring = $("#rawxml").val();   //server puts original xml in #rawxml

				    //remove declaration
				    xmlstring = xmlstring.substring(xmlstring.indexOf("&lt;FormDesign"));
				    $("#rawxml").val(xmlstring);
				    xmlDoc = $.parseXML(xmlstring);
				    $xml = $(xmlDoc);

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
				               

				        //find parentGuid
				        for (i = 0; i &lt; textboxes.length; i++) {
				            if (textboxes[i].type == "hidden") {
				                var components = textboxes[i].name.split(":");
				                parentId = components[1];

				                //remove repeat indicator if exists from Guid
				                if (parentId.indexOf(",") &gt; 0)
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
				        for (i = 0; i &lt; textboxes.length; i++) {

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
				                if (parentId.indexOf(",") &gt; 0)
				                    parentId = parentGuid.substring(0, parentId.indexOf(","));

				                ID = components[0].substring(1);

				               
				                if (textboxes[i].type == "hidden") {
				                   //note that parent has repeat indicator
				                    newname = "q" + ID + '-' + newguid + ":" + parentId;
				                }

				                
				                //find the element in the new table
				                var item;
				                var allelements = newtable.getElementsByTagName('*');
				                for(k=0;k &lt;allelements.length;k++)
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
				            for(m=0;m &lt;inputs.length;m++)
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
				    for (j = 0; j &lt; 32; j++) {
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
				    for(i=0; i &lt; tables.length; i++)
				    {
				       if(tables[i].id == sectionid) count++;
				    }
				   
				    return count;

				}

				function getLastRepeat(sectionid) {
				    var section = document.getElementById(sectionid);
				    var tables = section.parentElement.getElementsByTagName('TABLE');
				    var ret = null;
				    for(i=0;i &lt; tables.length;i++)
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
				    for(m=0;m &lt;inputs.length;m++)
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


				   for (i = 0; i &lt; children.length; i++) {

				      if (children[i].id == Id) {
				         return children[i];
				      }
				   }

				}

				function findElementByName(parentName, Name) {
				  //finds an element among descedants of a given node
				  var parent = document.getElementById(parentName);
				  var children = parent.getElementsByTagName('*');
				  
				  for (i = 0; i &lt; children.length; i++) {
				     if (children[i].name == Name) {
				         return children[i];
				     }
				  }
				}

				function xmlToString(xmlData) {

				    var xmlString;
				    
				    xmlString = (new XMLSerializer()).serializeToString(xmlData);
				    
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
				    var response = "&lt;response&gt;";
				    var html = response;

				    for (var i = 0; i &lt; elem.length; i++) {
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

				                response += "&lt;question ID=\"" + id + "\" display-name=\"" + value.replace(/&lt;/g, "&lt;").replace(/&gt;/g, "&gt;") 
				                         + "\"&gt;";
				                response += answer + "&lt;/question&gt;";               

				                q += "&lt;div class=\"MessageDataQuestion\"&gt;&lt;question ID=\"" + id + "\" display-name=\"" + value + "";
				                q += "&gt;&lt;br&gt;&lt;div class=\"MessageDataAnswer\"&gt;" + answer.replace(/&lt;/g,"&lt;").replace(/&gt;/g,"&gt;") + "&lt;/div&gt;&lt;/question&gt;&lt;/div&gt;";


				            }
				            sb += q;
				            answer = "";
				        }
				    }

				    response = response.replace(/&lt;br&gt;/g, "");
				    response = response + "&lt;/response&gt;";

				    flatXml = response;


				    sb = "&lt;div style='font-weight:bold; color:purple'&gt;Flat Xml response&lt;/div&gt;" 
				         + "&lt;div class=\"MessageDataChecklist\"&gt;&lt;response&gt;" + sb + "&lt;/response&gt;&lt;/div&gt;"
				         + "&lt;br/&gt;&lt;div style='font-weight:bold; color:purple'&gt;Response xml sent to web service.&lt;/div&gt;"


				    document.getElementById('MessageDataResult').innerHTML = sb;
				    document.getElementById('MessageData').style.display = 'block';
				    document.getElementById('FormData').style.display = 'none';
				    
				    
				    //update Xml with answers
				    updateXml();
				    
				  

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

				    for (var i = 0; i &lt; elem.length; i++) {
				        name = elem[i].name;
				        value = elem[i].value;

				        if (name.indexOf(qCkey) == 0) {

				            if (elem[i].checked || (elem[i].type == "text" &amp; value != "")) {

				                {

				                    var k = value.split(',');

				                    if (elem[i].type == "text" &amp; value != "") {
				                        //str += "&lt;answer value=\"" + value + "\"/&gt;&lt;br&gt;";
				                        str += "&lt;answer value=\"" + value + "\"/&gt;&lt;br&gt;";
				                    }
				                    else if (elem[i].type != "text") {
				                        //str += "&lt;answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/&gt;&lt;br&gt;";
				                        str += "&lt;answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/&gt;&lt;br&gt;";
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
				    if (strArray.length &gt; 1) {
				        for (var i = 1; i &lt; strArray.length; i++) {
				            if (i != strArray.length) {
				                returnStr += strArray[i] + ",";
				            }
				            else {
				                returnStr += strArray[i];
				            }
				        }
				    }
				    returnStr = returnStr.replace(/&lt;/g,"&lt;").replace(/&gt;/g,"&gt;");
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

				        if(questionid.indexOf(":")&gt;0)
				            questionid = questionid.substring(0,questionid.indexOf(':'));

				      
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

				function CallTestService(data) {
				    var webServiceURL = "http://localhost:5000/Service1.asmx";
				    webServiceURL = prompt("Enter endpoint:", webServiceURL);
				    
				    $.support.cors = true;
				    var xmldata = encodeURIComponent(data);
				    
				    var soapRequest =
									'&lt;?xml version="1.0" encoding="utf-8"?&gt;' +
										' &lt;soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"' +
												' xmlns:xsd="http://www.w3.org/2001/XMLSchema"' +
												' xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"&gt;' +
											' &lt;soap:Body&gt;' +
				                                    ' &lt;SubmitFormRequest xmlns="urn:ihe:iti:rfd:2007"&gt;' +
				                                    ' &lt;body&gt;' + xmldata + '&lt;/body&gt; ' +
				                                    ' &lt;/SubmitFormRequest&gt;' +
											' &lt;/soap:Body&gt;' +
										' &lt;/soap:Envelope&gt;';

				    
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


				function OnSuccess(data, status) {
					
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



				<!-- End -->

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
								<xsl:value-of select="$form-action"/>
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
							
							<xsl:if test="contains($form-action, 'http') or contains($form-action, 'javascript')">
								<!--remove submit button for the desktop verion-->
								
								<div class="SubmitButton">
									<input type="submit" value="Submit"/>
								</div>
							</xsl:if>
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