var xmlDoc;

$(document).ready(function () {

    jQuery.support.cors = true;

    //save original xml in jquery variable  
    var xmlstring = $("#rawxml").val();   //server puts original xml in #rawxml
    
    xmlDoc = $.parseXML(xmlstring);
    $xml = $(xmlDoc);

});

function sayHello() {
    alert("hello");

}

//adds a new repeat of a section
function addSection(obj) {
    //obj is btnAdd

    //we need to clone table, so get table
    var td = obj.parentElement;
    var table = td.parentElement  //tr
                   .parentElement  //tbody
                   .parentElement //table
    

    var tableid = table.id;
    var max = table.parentElement.firstChild.value;

    var textboxes = td.getElementsByTagName("input");  //get hidden input, radio buttons, checkboxes and input text boxes
    
    var newtable = table.cloneNode(true);    
    newtable.id = +tableid + 1;   //table id is repeat counter
    var repeat = newtable.id;

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
        parentsection.appendChild(newtable);

        //find parentGuid
        for (i = 0; i < textboxes.length; i++) {
            if (textboxes[i].type == "hidden") {
                var components = textboxes[i].name.split(":");
                parentGuid = components[1];

                //remove repeat indicator if exists from Guid
                if (parentGuid.indexOf(",") > 0)
                    parentGuid = parentGuid.substring(0, parentGuid.indexOf(","));
                break;
            }
        }

        /*add a new section node in xml by cloning the section with instanceGuid = parentGuid
        since there will be repeat sections with the same instanceGuid, take the last instance
        and clone it*/

        $section = $xml.find('Section[instanceGuid="' + parentGuid + '"]:last');
               
        sectionid = $section.attr('ID');
        newsec = $section.clone(true);
        
        //iterate through textboxes and assign new unique ids to them
        for (i = 0; i < textboxes.length; i++) {
            trace = 0;
            if (textboxes[i].type == "hidden" || textboxes[i].type == "text") {
                
                oldname = textboxes[i].name;
                var components = oldname.split(":");
                var newguid = generateGuid();

                if (i == 0) testguid = newguid;

                parentGuid = components[1];
                
                //remove repeat indicator if exists from Guid
                if (parentGuid.indexOf(",") > 0)
                    parentGuid = parentGuid.substring(0, parentGuid.indexOf(","));
                
                ID = components[2];

                //save guids to xml only for hidden (question) fields 
                //text fields are answer fields - no guids are required for answers
                if (textboxes[i].type == "hidden") {
                    //hidden textboxes have question ids
                    $question = newsec.find('Question[ID="' + ID + '"]');
                    $question.attr("instanceGuid", newguid);
                    $question.attr("parentGuid", parentGuid);
                }
                var $targetQuestion = newsec.find("Question[instanceGuid='" + newguid + "']");

                if (textboxes[i].type == "hidden") {
                   //note that parent has repeat indicator
                    newname = "q" + newguid + ":" + parentGuid + ",~" + repeat + ":" + ID;
                }
                var item = findElementByName(newtable.id, textboxes[i].name);

                if (item != null) {

                    item.name = newname;
                    if (item.type == "text") {
                        item.name = newname.substring(1);
                        item.value = "";

                    }
                    else {
                        item.name = newname;

                    }
                }

            }
        }



        //insert newsec after section
        $section.after(newsec);

        //hide the Add button if max count reached
        if (countRepeats(tableid) == max) {
            findElementById(newtable.id, "btnAdd").style.visibility = "hidden";
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
    var section = document.getElementById(sectionid);
    var tables = section.parentElement.getElementsByTagName('TABLE');
    return tables.length;

}

function getLastRepeat(sectionid) {
    var section = document.getElementById(sectionid);
    var tables = section.parentElement.getElementsByTagName('TABLE');
    return tables[tables.length - 1];

}

function removeSection(obj) {
    td = obj.parentElement;
    tr = td.parentElement;
    tbody = tr.parentElement;
    table = tbody.parentElement;
    var section = table.parentElement;
    section.removeChild(table);

    last = getLastRepeat(section.id);

    findElementById(last.id, "btnAdd").style.visibility = "visible";

    //to do remove repeat node from xml
    //
    
    instanceGuid = section.id;
    repeat = table.id;
    //$question = newsec.find('Question[ID="' + ID + '"]');
    $xml.find('Section[ID="' + instanceGuid + '"]').eq(repeat).remove(); 
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

    for (var i = 0; i < elem.length; i++) {
        q = "";
        var name = elem[i].name;

        var value;

        //split up name into instanceGuid and parentGuid and question id
        var components = name.split(':');
        var instanceGuid = components[0].substring(1);
        var parentGuid = components[1];
        var id = components[2];

        if (name.indexOf("q") == 0) {
            value = elem[i].value;

            answer = GetAnswer(name.substring(1));

            if (answer != "") {

                response += "<question ID=\"" + id + "\" display-name=\"" + value + "\" instanceGuid=\"" + instanceGuid + "\" parentGuid = \"" + parentGuid + "\">";
                response += answer.replace(/&lt;/g, "<").replace(/&gt;/g, ">") + "</question>";

                q += "<div class=\"MessageDataQuestion\">&lt;question ID=\"" + id + "\" display-name=\"" + value + "\" instanceGuid=\"" + instanceGuid + "\" parentGuid = \"" + parentGuid + "\"";
                q += "&gt;<br><div class=\"MessageDataAnswer\">" + answer + "</div>&lt;/question&gt;</div>";


            }
            sb += q;
            answer = "";
        }
    }

    response = response.replace(/<br>/g, "");
    response = response + "</response>";

    flatXml = response;

    sb = "<div class=\"MessageDataChecklist\">&lt;response&gt;" + sb + "&lt;/response&gt;</div>"

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
                        str += "&lt;answer value=\"" + value + "\"/&gt;<br>";
                    }
                    else if (elem[i].type != "text") {
                        str += "&lt;answer ID=\"" + k[0] + "\" display-name=\"" + GetDisplayName(value) + "\"/&gt;<br>";
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
    return returnStr.substr(0, returnStr.length - 1);
}


//updates answers in full xml
function updateXml() {
    FlatDoc = $.parseXML(flatXml);
    $xmlFlatDoc = $(FlatDoc);
    $xmlFlatDoc.find('question').each(function () {
        var $question = $(this);
        var guid = $question.attr("instanceGuid");
        var parentGuid = $question.attr("parentGuid");
        var repeat = 0;
        if (parentGuid.indexOf("~") > 0)
            repeat = parentGuid.split('~')[1];
        if (repeat > 0)
            repeat = +repeat - 1;

        //there may be multiple answers per question
        $question.find('answer').each(function () {
            var $test = $(this);
            var id = $test.attr("ID");
            var val = $test.attr("value");

            var $xml = $(xmlDoc);  //full xml
            var $targetQuestion = $xml.find("Question[instanceGuid='" + guid + "']");
            var targetQuestionId = $targetQuestion.attr("ID");

            if (id != null) {
                var $targetAnswer = $targetQuestion.find("ListItem[ID='" + id + "']");
                $targetAnswer.attr("selected", "true");
                if ($targetAnswer.find("ListItemResponseField") != null) {
                    val = $question.find('answer').next().attr("value");
                    $response = $targetAnswer.find("Response").children(0);
                    $response.attr("val", val);
                }

            }
            else {
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
					'<?xml version="1.0" encoding="utf-8"?>' +
						' <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"' +
								' xmlns:xsd="http://www.w3.org/2001/XMLSchema"' +
								' xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">' +
							' <soap:Body>' +
    								//' <Add xmlns="urn:ihe:iti:rfd:2007">' +
									//' <i>4</i>' +
									//' <j>5</j>' +
								    //' </Add>' +
                                    //' </test>' +
                                    ' <SubmitFormRequest xmlns="urn:ihe:iti:rfd:2007">' +
                                    ' <body>' + xmldata + '</body> ' +
                                    ' </SubmitFormRequest>' +
							' </soap:Body>' +
						' </soap:Envelope>';

    
    soapAction = "urn:SubmitForm";
    //soapAction = "urn:ihe:iti:rfd:2007/Add";

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
    if (document.getElementById("outxml") != null) {
        alert(xmlstring);
        document.getElementById("outxml").textContent = xmlstring;
        $("#outxml").css("background-color", "yellow");
    }
    
    
}

function OnError(xhr, textStatus, err) {
    alert('Status:' + xhr.status + ', Text:' + textStatus + ', Error: ' + err);
}

