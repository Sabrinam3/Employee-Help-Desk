/*
 * Name: GenerateReports
 * Purpose: generates reports when user clicks buttons
 * Coder: Sabrina Tessier
 * Date: Nov 25/18
*/

$(function () {
//Generate employee report when button clicked
    $('#employeebutton').click(async (e) => {
        try {
            $('#reportstatus').text('generating report on server - please wait...');
            let response = await fetch(`api/employeereport`);
            if (!response.ok)
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            if (data == 'report generated') {
                window.open('/Pdfs/Employee.pdf');
                $('#reportstatus').text('Report generated');
            }
            else {
                $('#reportstatus').text('problem generating report');
            }  
        } catch (error) {
            $('#reportstatus').text(error.message);
        }
    });
//Generate call report when button clicked
    $('#callbutton').click(async (e) => {
        try {
            $('#reportstatus').text('generating report on server - please wait...');
            let response = await fetch(`api/callreport`);
            if (!response.ok)
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            if (data == 'report generated') {
                window.open('/Pdfs/Call.pdf');
                $('#reportstatus').text('Report generated');
            }
            else {
                $('#reportstatus').text('problem generating report');
            }  
        } catch (error) {
            $('#reportstatus').text(error.message);
        }
    });
});//end jquery