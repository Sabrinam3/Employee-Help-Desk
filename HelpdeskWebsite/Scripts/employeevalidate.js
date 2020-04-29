//Javascript functions to handle fetching the employees from the server and building the list. Also to handle the add, update and delete functionality.

$(function () {
    //Method that gets all the data from the server and calls the buildEmployeeList method
    const getAll = async (msg) => {
        try {
            $('#employeeList').html('<h3>Finding Employee Information, please wait...</h3>');
            let response = await fetch(`api/employees/`);
            if (!response.ok)
                throw new Error(`Status- ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            buildEmployeeList(data, true);
            //If the message is blank, add 'Employees loaded', if the message is not blank, append 'Employees Loaded' to the previous message that was in the status div
            (msg === '') ?
                $('#status').text('Employees Loaded') : $('#status').text(`${msg} - Employees Loaded`);

            //Fetch all the departments from the server and place them into local storage
            response = await fetch(`api/departments`);
            if (!response.ok)
                throw new Error(`Status- ${response.status}, Text - ${response.statusText}`);
            let deps = await response.json();
            localStorage.setItem('alldepartments', JSON.stringify(deps));
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end getAll   

    //Loads the dropdown list of departments
    const loadDepartmentDDL = (empDepId) => {
        html = '';
        $('#ddlDeps').empty(); //empty out the existing items
        let alldepartments = JSON.parse(localStorage.getItem('alldepartments')); //retrieve the JSON string from local storage and parse it back into an array 
        //For every department, set the html string
        alldepartments.map(dep => html += `<option value="${dep.Id}">${dep.Name}</option>`);
        //append the html string to the dropdown which creates an option for each
        $('#ddlDeps').append(html);
        //Display the 'start up' option
        $('#ddlDeps').val(empDepId);
    }//end loadDepartmentDDL

    //Method that sets the value of the action button to update, sets the modal status div and title to reflect the update functionality, clears the modal fields of existing data and populates
    //the fields with the existing data for the employee the user clicked on
    const setupForUpdate = (Id, data) => {
        //Set the value of the action button to update so that we can use this to call the update method
        $('#actionbutton').val('Update');
        //Set the modal title
        $('#modaltitle').html('<h4>update employee</h4>');
        //Clear out the existing fields in the modal and remove Id, DepartmentId, and Timer values from local storage
        clearModalFields();
        //iterate through each employee in the data array until the employee with the matching id is found.
        //Then, set the values in the modal so when the modal opens, all fields are populated
        data.map(employee => {
            if (employee.Id === parseInt(Id)) {
                $('#TextBoxTitle').val(employee.Title);
                $('#TextBoxFirstname').val(employee.Firstname);
                $('#TextBoxLastname').val(employee.Lastname);
                $('#TextBoxPhone').val(employee.Phoneno);
                $('#TextBoxEmail').val(employee.Email);
                $('#ImageHolder').html(`<img height="120" width="110" src="data:image/png;base64,${employee.StaffPicture64}"/>`)

                //Call the loadDepartmentDDL method to populate the dropdown list.
                //Pass in the selected employee's division so that this option is highlighted when the modal loads
                loadDepartmentDDL(employee.DepartmentId);

                //Place the employee's Id, DepartmentId, and Timer into local storage
                localStorage.setItem('Id', employee.Id);
                localStorage.setItem('DepartmentId', employee.DepartmentId);
                localStorage.setItem('Timer', employee.Timer);
                localStorage.setItem('Picture', employee.StaffPicture64);
                $('#modalstatus').text('update data');//set the status div of the modal
                /*Show the generate report button*/
                $('#reportbutton').show();
                $('#theModal').modal('toggle');//pop the modal
            }//end if
        }); //data.map
    }//end setupForUpdate

    //Method that sets the value of the action button to 'add', sets the modal status div and modal title to reflect the add functionality. Then clear the modal fields.
    const setupForAdd = () => {
        $('#actionbutton').val('Add'); //sets the value property of the actionbutton to be add
        $('#modaltitle').html('<h4>add employee</h4>'); //sets the title of the modal to be 'add employee'
        $('#modalstatus').text('add new employee');//set the status div of the modal
        clearModalFields();//clear all existing data from the modal fields
        /*Hide the generate report button because we don't need it for adding*/
        $('#reportbutton').hide();
        $('#theModal').modal('toggle');//pop the modal
    }//end setupForAdd

    //Clears out all the modal fields and removes Id, DepartmentId, and Timer values from local storage
    const clearModalFields = () => {
        //Set the text boxes in the modal to be blank
        $('#TextBoxTitle').val('');
        $('#TextBoxFirstname').val('');
        $('#TextBoxLastname').val('');
        $('#TextBoxPhone').val('');
        $('#TextBoxEmail').val('');
        $('#ImageHolder').html(`<img src=''"/>`)
        //Load the options into the dropdown list. Pass -1 so that when the modal opens, the dropdown list is blank
        loadDepartmentDDL(-1);
        //Remove current properties from local storage
        localStorage.removeItem('Id');
        localStorage.removeItem('DepartmentId');
        localStorage.removeItem('Timer');
        localStorage.removeItem('Picture');
    }//end clearModalFields

    //Method that takes the list of employees from local storage and builds a list of employees by creating buttons that are populated with each employee's Title, first name, and last name.
    let buildEmployeeList = (data, allemployees) => {
        $('#employeeList').empty(); //Clear out the div
        //Create the 'top' of the div which displays the title 'Employee Info' and the headings
        div = $(`<div class="list-group-item row d-flex" id="status">Employee Info</div>
                      <div class= "list-group-item row d-flex text-center" id="heading">
                       <div class="col-4 h4">Title</div>
                       <div class="col-4 h4">First</div>
                       <div class="col-4 h4">Last</div>
                        </div>`);
        div.appendTo($('#employeeList'))//Append the previously created headings to the employeeList div

        //Place the data (i.e. the JSON array of employee objects) into local storage after turning it into a JSON string(if the boolean is true)
        allemployees ? localStorage.setItem('allemployees', JSON.stringify(data)) : null;

        //Create the button that the user can click to add a new employee
        btn = $(`<button class = "list-group-item row d-flex" id="0"><div class="col-12 text-left" id="clicktoadd">...click to add employee </div></button>`);
        btn.appendTo($('#employeeList'));

        //Iterate through each employee in the data array and for each employee, create a button that has three divs-> each of which has an Id set to be the employee's Id
        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.Id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.Id}">${emp.Title}</div>
                            <div class="col-4" id="employee${emp.Id}">${emp.Firstname}</div>
                            <div class="col-4" id="employeelastname${emp.Id}">${emp.Lastname}</div>`
            );
            btn.appendTo($('#employeeList'));
        }); //map
    }//end buildEmployeeList

    //Method that builds an updated instance of employee based on the data the user has entered into the modal fields. Then sends the information back through the layers and into the database.
    const update = async () => {
        try {
            //Create a local instance of an employee object and set the properties based on the values from the modal's text boxes
            emp = new Object();
            emp.Title = $('#TextBoxTitle').val();
            emp.Firstname = $('#TextBoxFirstname').val();
            emp.Lastname = $('#TextBoxLastname').val();
            emp.Phoneno = $('#TextBoxPhone').val();
            emp.Email = $('#TextBoxEmail').val();
            emp.DepartmentId = $('#ddlDeps').val();
            //Pull the Id and Timer properties from local storage to apply to the employee being created
            emp.Id = localStorage.getItem("Id");
            emp.Timer = localStorage.getItem('Timer');
            localStorage.getItem('Picture') ? emp.StaffPicture64 = localStorage.getItem('Picture') : null;
            //send the employee to the server asynchronously using PUT
            let response = await fetch('api/employees', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) {
                let data = await response.json(); //goes to the Employee controller and returns message from there
                //Refresh the data so that the changes are reflected in the list when the modal closes
                getAll(data);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            }
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end update

    //Method that builds a new instance of employee based on the data the user has entered into the modal fields. Then sends the information back through the layers and into the database.
    const add = async () => {
        try {
            //Creates a local instance of employee and populates the properties with the values from the modal's textboxes
            emp = new Object();
            emp.Title = $('#TextBoxTitle').val();
            emp.Firstname = $('#TextBoxFirstname').val();
            emp.Lastname = $('#TextBoxLastname').val();
            emp.Phoneno = $('#TextBoxPhone').val();
            emp.Email = $('#TextBoxEmail').val();
            emp.DepartmentId = $('#ddlDeps').val();
            emp.StaffPicture64 = localStorage.getItem('Picture');
            emp.Id = -1;
            //send the employee into to the server asynchronously using POST
            let response = await fetch('api/employees', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data);
            } else {
                $('#status').text(`${response.status}, Text - ${response.statusText}`);
            }
            $('#theModal').modal('toggle'); //hide the modal
        } catch (error) {
            $('#status').text(error.message);
        }
    }//end Add

    //Method that when the user clicks on an employee and then clicks the delete button, will delete that employee from the database by using the Id of the employee the user has clicked on.
    let _delete = async () => {
        try {
            //Get the instance of employee the user has clicked on using the Id from local storage.
            let response = await fetch(`api/employees/${localStorage.getItem('Id')}`, {
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

    //The validate method - outlines the rules for the text boxes
    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need vaild email format"
            }
        }
    });

    $.validator.addMethod("validTitle", function (value, element) { // custom rule
        return this.optional(element) || (value == "Mr." || value == "Ms." || value == "Mrs." || value == "Dr.");
    }, "");
    //When the action button is clicked, determine whether it's current value is update or add and call the appropriate method
    $("#actionbutton").click((e) => {
        if ($("#EmployeeModalForm").valid())
            $("#actionbutton").val() === "update" ? update() : add();
        else {
            $("#modalstatus").text("Fix Errors");
            e.preventDefault;
        }
    }); //actionbutton click

    //Code that runs the confirmation popup for the delete button.
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });

    $('#deletebutton').click(() => _delete()); //if yes was chosen, call the delete function

    //When the employee list is clicked, assign the appropriate id based on where the user clicked
    $("#employeeList").click((e) => {
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === 'employeeList' || Id === '') {
            Id = e.target.id;
        }//clicked on row somewhere else

        //If the user has clicked on a real row, retrieve the array of employees from local storage and turn back into a working array of objects by using JSON.parse
        if (Id !== 'status' && Id !== 'heading') {
            let data = JSON.parse(localStorage.getItem('allemployees'));
            Id === '0' ? setupForAdd() : setupForUpdate(Id, data); //if the Id is 0, the user is trying to perform an add operation so call the add setup method, else call the update setup.
        } else {
            return false; //ignore if they clicked on heading or status
        }
    });

    /*Generates an employee report*/
    let generateReport = async () => {
        /*Retrieve the id for the currently selected employee from local storage*/
        let id = localStorage.getItem('Id');
        try {
            $('#modalstatus').text('generating report on server - please wait...');
            let response = await fetch(`api/callreport/${id}`);
            if (!response.ok)
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            if (data == 'report generated') {
                window.open('/Pdfs/EmployeeCall.pdf');
                $('#modalstatus').text('Report generated');
            }
            else {
                $('#modalstatus').text('problem generating report');
            }
        } catch (error) {
            $('#modalstatus').text(error.message);
        }
    }

    //When the generate report button is clicked, generate a report for that employee
    $("#reportbutton").click((e)  => {
       /*Call the generate report function*/
        generateReport();
    });

    //Filters the data based on what's in the search bar
    const filterData = () => {
        allData = JSON.parse(localStorage.getItem('allemployees'));
        //tilde below same as stu.Lastname.indexOf($('#srch).val > -1)
        let filteredData = allData.filter((emp) => ~emp.Lastname.indexOf($('#srch').val()));
        buildEmployeeList(filteredData, false);
    }//end filterData
    getAll(''); //first grab the data from the server
    $('#srch').keyup(filterData); //search key press

    //Do we have a picture?
    $('input:file').change(() => {
        const reader = new FileReader();
        const file = $('#fileUpload')[0].files[0];

        file ? reader.readAsBinaryString(file) : null;

        reader.onload = (readerEvt) => {
            //get binary data then convert to encoded string
            const binaryString = reader.result;
            const encodedString = btoa(binaryString);
            localStorage.setItem('Picture', encodedString);
        }
    });//input file change
}); //jQuery ready method