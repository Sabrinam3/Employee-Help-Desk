/*
 Javascript functions to handle fetching the calls from the server and building the list.Also to handle the add, update and delete functionality.
 Coder: Sabrina Tessier
 Date: Nov 19/2018
*/

$(function () {
    //Method that gets all the data from the server and calls the buildCallList method
    const getAll = async (msg) => {
        try {
            $('#callList').html('<h3>Finding Call Information, please wait...</h3>');
            let response = await fetch(`api/calls/`);
            if (!response.ok)
                throw new Error(`Status- ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            buildCallList(data, true);
            //If the message is blank, add 'Employees loaded', if the message is not blank, append 'Employees Loaded' to the previous message that was in the status div
            (msg === '') ?
                $('#status').text('Calls Loaded') : $('#status').text(`${msg} - Calls Loaded`);

           //Fetch all problems from the server and load them into local storage
            response = await fetch(`api/problems`);
            if (!response.ok)
                throw new Error(`Status- ${response.status}, Text - ${response.statusText}`);
            let probs = await response.json();
            localStorage.setItem('allproblems', JSON.stringify(probs));
            //Fetch all employees from the server and place them into local storage
            response = await fetch('api/employees');
            if (!response.ok)
                throw new Error(`Status- ${response.status}, Text - ${response.statusText}`);
            let emps = await response.json();
            localStorage.setItem('allemployees', JSON.stringify(emps));          
        } catch (error) {
            $('#status').text(error.message);
        }   
    }//end getAll   
   
    //Method that takes the list of calls and builds a list of calls by creating buttons that are populated with each call's properties
    let buildCallList = (data, allcalls) => {
        $('#callList').empty(); //Clear out the div
        //Create the 'top' of the div which displays the title 'Call Info' and the headings
        div = $(`<div class="list-group-item row d-flex" id="status">Call Info</div>
                      <div class= "list-group-item row d-flex text-center" id="heading">
                       <div class="col-4 h4">Date</div>
                       <div class="col-4 h4">For</div>
                       <div class="col-4 h4">Problem</div>
                        </div>`);
        div.appendTo($('#callList'))//Append the previously created headings to the callList div

        //Place the data (i.e. the JSON array of Call objects) into local storage after turning it into a JSON string(if the boolean is true)
        allcalls ? localStorage.setItem('allcalls', JSON.stringify(data)) : null;

        //Create the button that the user can click to add a new call
        btn = $(`<button class = "list-group-item row d-flex" id="0"><div class="col-12 text-left" id="clicktoadd">...click to add call </div></button>`);
        btn.appendTo($('#callList'));

        //Iterate through each call in the data array and for each call, create a button that has three divs-> each of which has an Id set to be the call's Id
        data.map(call => {
            btn = $(`<button class="list-group-item row d-flex" id="${call.Id}">`);
            btn.html(`<div class="col-4" id="callopendate${call.Id}">${formatDate(call.DateOpened)}</div>
                            <div class="col-4" id="callemployee${call.Id}">${call.EmployeeName}</div>
                            <div class="col-4" id="callproblem${call.Id}">${call.ProblemDescription}</div>`
            );
            btn.appendTo($('#callList'));
        }); //map
    }//end buildCallList

    //When the call list is clicked, assign the appropriate id based on where the user clicked
    $("#callList").click((e) => {
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === 'callList' || Id === '') {
            Id = e.target.id;
        }//clicked on row somewhere else

        //If the user has clicked on a real row, retrieve the array of calls from local storage and turn back into a working array of objects by using JSON.parse
        if (Id !== 'status' && Id !== 'heading') {
            let data = JSON.parse(localStorage.getItem('allcalls'));
            Id === '0' ? setupForAdd() : setupForUpdate(Id, data); //if the Id is 0, the user is trying to perform an add operation so call the add setup method, else call the update setup.
        } else {
            return false; //ignore if they clicked on heading or status
        }
    });//end click event handler

    //Loads the dropdown list of problems
    const loadProblemsDDL = (problemId) => {
        html = '';
        $('#ddlProblems').empty(); //empty out the existing items
        let allproblems = JSON.parse(localStorage.getItem('allproblems')); //retrieve the JSON string from local storage and parse it back into an array 
        //For every problem, set the html string
        allproblems.map(prob => html += `<option value="${prob.Id}">${prob.Description}</option>`);
        //append the html string to the dropdown which creates an option for each
        $('#ddlProblems').append(html);
        //Display the 'start up' option
        $('#ddlProblems').val(problemId);
    }//end loadProblemsDDL

    //Loads the dropdown list of employees
    const loadEmployeesDDL = (empid) => {
        html = '';
        $('#ddlEmployees').empty(); //empty out the existing items
        let allemployees = JSON.parse(localStorage.getItem('allemployees')); //retrieve the JSON string from local storage and parse it back into an array 
        //For every problem, set the html string
        allemployees.map(emp => html += `<option value="${emp.Id}">${emp.Lastname}</option>`);
        //append the html string to the dropdown which creates an option for each
        $('#ddlEmployees').append(html);
        //Display the 'start up' option
        $('#ddlEmployees').val(empid);
    }//end loadEmployeesDDL

    //Loads the dropdown list of technicians
    const loadTechniciansDDL = (techid) => {
        html = '';
        $('#ddlTechnicians').empty(); //empty out the existing items
        let allemployees = JSON.parse(localStorage.getItem('allemployees')); //retrieve the JSON string from local storage and parse it back into an array
        var technicians = [];
        for (var i = 0; i < allemployees.length; ++i)
        {
            if (allemployees[i].isTech === true)
              technicians.push(allemployees[i]);
        }
        ////For every technician, set the html string
        technicians.map(tech => html += `<option value="${tech.Id}">${tech.Lastname}</option>`);
        //append the html string to the dropdown which creates an option for each
        $('#ddlTechnicians').append(html);
        //Display the 'start up' option
        $('#ddlTechnicians').val(techid);
    }//end loadTechniciansDDL

    //Clears out all the modal fields and resets the dropdowns
    const clearModalFields = () => {
        //Load the problems into the dropdown list. Pass -1 so that when the modal opens, the dropdown list is blank
        loadProblemsDDL(-1);
        //Load the employees into the dropdown list. Pass -1 so that when the modal opens, the dropdown list is blank
        loadEmployeesDDL(-1);
        //  //Load the technicians into the dropdown list. Pass -1 so that when the modal opens, the dropdown list is blank
        loadTechniciansDDL(-1);
        //Set the text boxes in the modal to be blank
        $('#DateOpenedLabel').text('');
        $('#dateOpened').val('');
        $('#DateClosedLabel').text('');
        $('#dateClosed').val('');
        $('#NotesBox').val('');
        //Uncheck the check box
        $('#OpenStatusCheckBox').prop('checked', false);
    }//end clearModalFields


    //Method that sets the value of the action button to update, sets the modal status div and title to reflect the update functionality, clears the modal fields of existing data and populates
    //the fields with the existing data for the call the user clicked on
    const setupForUpdate = (Id, data) => {
        //Set the value of the action button to update so that we can use this to call the update method
        $('#actionbutton').val('update');
        //Set the modal title
        $('#modaltitle').html('<h4>Update Call Information</h4>');
        //Clear out the existing fields in the modal
        clearModalFields();
        //iterate through each call in the data array until the call with the matching id is found.
        //Then, set the values in the modal so when the modal opens, all fields are populated
        data.map(call => {
            if (call.Id === parseInt(Id)) {
                $('#ddlProblems').val(call.ProblemId);
                $('#ddlEmployees').val(call.EmployeeId);
                $('#ddlTechnicians').val(call.TechId);
                $('#labelDateOpened').text(formatDate(call.DateOpened));
                $('#NotesBox').val(call.Notes);
  
                //Add the right instance of call into local storage
                localStorage.setItem('currentCall', JSON.stringify(call));
                $('#modalstatus').text('update data');//set the status div of the modal

                if (!call.OpenStatus) {
                    //Check the checkbox to be checked
                    $('#OpenStatusCheckBox').prop('checked', true);
                    //Display the closed date
                    $('#labelDateClosed').text(formatDate(call.DateClosed));
                    $('#dateClosed').val(formatDate(call.DateClosed));
                   // disable everything but the delete button
                    $('#ddlProblems').prop('disabled', true);
                    $('#ddlEmployees').prop('disabled', true);
                    $('#ddlTechnicians').prop('disabled', true);
                    $('#OpenStatusCheckBox').prop('disabled', true);
                    $('#NotesBox').attr('readonly', 'readonly');
                    $('#actionbutton').prop('disabled', true);
                    $('#deletebutton').prop('disabled', false);
                }
                else {
                    $('#labelDateClosed').text('');
                    $('#dateClosed').val('');
                    $('#ddlProblems').prop('disabled', false);
                    $('#ddlEmployees').prop('disabled', false);
                    $('#ddlTechnicians').prop('disabled', false);
                    $('#OpenStatusCheckBox').prop('disabled', false);
                    $('#NotesBox').attr('readonly', false);
                    $('#actionbutton').prop('disabled', false);
                    $('#deletebutton').prop('disabled', false);
                }
                $('#theModal').modal('toggle');//pop the modal
            }//end if
        }); //data.map
    }//end setupForUpdate

    //Method that builds an updated instance of employee based on the data the user has entered into the modal fields. Then sends the information back through the layers and into the database.
    const update = async () => {
        try {
            //Create a local instance of an employee object and set the properties based on the values from the modal's text boxes
            let currentCall = JSON.parse(localStorage.getItem('currentCall'));
            currentCall.ProblemId = $('#ddlProblems').val();
            currentCall.EmployeeId = $('#ddlEmployees').val();
            currentCall.TechId = $('#ddlTechnicians').val();
            currentCall.OpenStatus = !$('#OpenStatusCheckBox').is(':checked');
            currentCall.Notes = $('#NotesBox').val();
            currentCall.DateClosed = $('#labelDateClosed').text();
           
            //send the employee to the server asynchronously using PUT
            let response = await fetch('api/calls', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(currentCall)
            });
            if (response.ok) {
                let data = await response.json(); //goes to the call controller and returns message from there
                //Refresh the data so that the changes are reflected in the list when the modal closes
                getAll(data, true);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            }
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end update

    //Method that sets the value of the action button to 'add', sets the modal status div and modal title to reflect the add functionality. Then clear the modal fields.
    const setupForAdd = () => {
        $('#actionbutton').val('add'); //sets the value property of the actionbutton to be add
        $('#modaltitle').html('<h4>add call</h4>'); //sets the title of the modal to be 'add call'
        $('#modalstatus').text('add new call');//set the status div of the modal
        clearModalFields();//clear all existing data from the modal fields
        //populate the Open Date field with the current date/time
        $('#labelDateOpened').text(formatDate());
        $('#dateOpened').val(formatDate());
        $('#labelDateClosed').text('');
        $('#dateClosed').val('');
        //Set the checkbox to not be checked
        $('#OpenStatusCheckBox').prop('checked', false);
        //Disable the checkbox
        $('#OpenStatusCheckBox').prop('disabled', true);
        //Re-enable other elements
        $('#ddlProblems').prop('disabled', false);
        $('#ddlEmployees').prop('disabled', false);
        $('#ddlTechnicians').prop('disabled', false);
        $('#NotesBox').attr('readonly', false);
        $('#actionbutton').prop('disabled', false);
        $('#deletebutton').prop('disabled', false);
        $('#theModal').modal('toggle');//pop the modal
    }//end setupForAdd

    //Method that builds a new instance of employee based on the data the user has entered into the modal fields. Then sends the information back through the layers and into the database.
    const add = async () => {
        try {
            //Creates a local instance of call and populates the properties with the values from the modal's textboxes
            call = new Object();
            call.ProblemId = $('#ddlProblems').val();
            call.EmployeeId = $('#ddlEmployees').val();
            call.TechId = $('#ddlTechnicians').val();
            call.OpenStatus = !$('#OpenStatusCheckBox').is('checked');
            call.Notes = $('#NotesBox').val();
            call.DateOpened = $('#labelDateOpened').text();
            call.Id = -1;
            //send the employee into to the server asynchronously using POST
            let response = await fetch('api/calls', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(call)
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data, true);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            }
            $('#theModal').modal('toggle'); //hide the modal
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end Add

    //When the action button is clicked, determine whether it's current value is update or add and call the appropriate method
    $("#actionbutton").click((e) => {
        $('#modalStatus').removeClass();
        if ($("#CallModalForm").valid())
            $("#actionbutton").val() === "update" ? update() : add();
        else {
            $("#modalstatus").text("Fix Errors");
            e.preventDefault;
        }
    }); //actionbutton click

    //Code that runs the confirmation popup for the delete button.
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });

    $('#deletebutton').click(() => _delete()); //if yes was chosen, call the delete function

    //Method that when the user clicks on a call and then clicks the delete button, will delete that call from the database by using the Id of the call the user has clicked on.
    let _delete = async () => {
        try {
            //Get the instance of call the user has clicked on using the Id from local storage.
            let currentCall = JSON.parse(localStorage.getItem('currentCall'));
            let response = await fetch(`api/calls/${currentCall.Id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                }
            });
            if (response.ok) {
                let data = await response.json();
                //Refresh the list to show changes
                getAll(data);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            }//else
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end delete

    //Checkbox click listener
    $('#OpenStatusCheckBox').click(() => {
        if ($('#OpenStatusCheckBox').is(':checked')) {
            $('#labelDateClosed').text(formatDate());
            $('#dateClosed').val(formatDate());
        }
        else {
            $('#labelDateClosed').text('');
            $('#dateClosed').val('');
        }
    });//checkBoxClick

    const formatDate = (date) => {
        let d;
        date === (undefined) ? d = new Date() : d = new Date(Date.parse(date));
        let _day = d.getDate();
        let _month = d.getMonth() + 1;
        let _year = d.getFullYear();
        let _hour = d.getHours();
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }
        if (_year > 2020) return "";
        return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    };

    //The validate method - outlines the rules for the text boxes
    $("#CallModalForm").validate({
        rules: {
            ddlProblems: {required: true},
            ddlEmployees: { required: true },
            ddlTechnicians: { required: true },
            NotesBox: {maxlength:250, required: true }           
        },
        errorElement: "div",
        messages: {
            ddlProblems: {
                required: "select Problem"
            },
            ddlEmployees: {
                required: "select Employee"
            },
            ddlTechnicians: {
                required: "select Tech"
            },
            NotesBox: {
                required: "required 1-250 chars.", maxlength: "required 1-250 chars."
            }
        }
    });

    //Filters the data based on what's in the search bar
    const filterData = () => {
        allData = JSON.parse(localStorage.getItem('allcalls'));
        let filteredData = allData.filter((call) => ~call.EmployeeName.indexOf($('#srch').val()));
        buildCallList(filteredData, false);
    }//end filterData
    getAll(''); //first grab the data from the server
    $('#srch').keyup(filterData); //search key press


    getAll(''); //first grab the data from the server
});//jquery ready method

