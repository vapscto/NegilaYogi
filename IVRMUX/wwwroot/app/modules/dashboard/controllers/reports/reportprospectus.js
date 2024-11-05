

(function () {
    'use strict';
    angular
.module('app')
.controller('ReportProspectusController', ReportProspectusController)

    ReportProspectusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter','superCache']
    function ReportProspectusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        //Date:23-12-2016 for displaying privileges.
        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.pagination = false;
        $scope.validate = false;
        $scope.sms_flag = false;
        $scope.mail_flag = false;
        $scope.export_flag = false;
        $scope.print_flag = false;
        $scope.errMessage_From_Date = 'Select From Date from the Calendar';
        $scope.errMessage_To_Date = 'Select To Date from the Calendar';
        $scope.errMessage_Year = 'Select Academic Year';
        $scope.propertyName = 'pasP_FirstName';

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.pasP_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || JSON.stringify(obj.pasP_MobileNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.pasP_Phone).indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pasP_FirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasP_MiddleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasP_LastName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasP_emailid)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.state)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasP_ProspectusNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        //export start

        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }
        //export end


        // print start
        //$scope.printData = function () {
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //        var divToPrint = document.getElementById("table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //    }
        //    else {
        //        swal("Please select records to be printed");
        //    }
        //}

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
           );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


        //print end

        //Sorting function
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
       

        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //Loading the Intial Data Function
        $scope.loaddata = function () {
            apiService.getURI("ReportProspectus/get_intial_data", pageid).
        then(function (promise) {
            
            $scope.yearlst = promise.fillyear;
        })
        }

        //Collapse function
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }



        //Radio button switching function
        $scope.onclickloaddata = function () {
            if ($scope.enq.yearwiseorbtwdates === 'yearwise') {
                $scope.enq.ASMAY = "";
                $scope.frdatetodate = true;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.IsHiddendown = false;
                $scope.submitted = false;
                
            }
            else if ($scope.enq.yearwiseorbtwdates === 'btwdates') {
               
                $scope.errMessage = "";
                $scope.enq.FromDate = "";
                $scope.enq.ToDate = "";
                $scope.frdatetodate = false;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.submitted = false;
                $scope.IsHiddendown = false;
            }
        };


        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }



        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
        }



        $scope.optionToggled = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.reportdetails.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

        }


        //Date Compare function
        $scope.checkErr = function (FromDate, ToDate) {
            
            $scope.errMessage = '';
            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.IsHiddendown = false;
                $scope.errMessage = 'To Date should be greater than from date';
              
                return false;
            }
           
        };

        //date field Validation
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        
      



        //Report showing function
        $scope.ShowReport = function (enq) {
            
            
            
            $scope.submitted = false;
            //Form validation
            if ($scope.myForm.$valid) {
                if ($scope.enq.yearwiseorbtwdates === "yearwise") {
                    $scope.enq.FromDate = "";
                    $scope.enq.ToDate = "";
                    $scope.enq.ASMAY = $scope.enq.ASMAY;
                }
                else if ($scope.enq.yearwiseorbtwdates === "btwdates") {
                    $scope.enq.FromDate = new Date($scope.enq.FromDate).toDateString(),

                        $scope.enq.ToDate = new Date($scope.enq.ToDate).toDateString(),
                    $scope.enq.ASMAY = 0;
                }
            var data = {
                "MI_Id": 2,
                "From_Date": $scope.enq.FromDate,
                "To_Date": $scope.enq.ToDate,
                "ASMAY_Id": $scope.enq.ASMAY,
             
            };
            
            apiService.create("ReportProspectus/Getdetails", data).
   then(function (promise) {
       
       if (promise.searchstudentDetails!=null && promise.searchstudentDetails.length > 0)
       {
         
          
           $scope.pagination = true;
           $scope.IsHiddendown = true;
           $scope.sms_flag = true;
           $scope.mail_flag = true;
           $scope.export_flag = true;
           $scope.print_flag = true;
           $scope.reportdetails = promise.searchstudentDetails;
           $scope.presentCountgrid = promise.searchstudentDetails.length;
       }
       else
       {
           swal("No Records Found")
           $scope.pagination = false;
           $scope.IsHiddendown = false;
           $scope.print_flag = false;
           $scope.sms_flag = false;
           $scope.mail_flag = false;
           $scope.export_flag = false;

          
       }
      
   })
           
            }
            else {
                $scope.submitted = true;

            }
        };



        //SMS Sending function
        $scope.SendMSG = function (Text) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Send SMS?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    $scope.albumNameArray = [];
                    angular.forEach($scope.reportdetails, function (user) {
                        if (!!user.selected) $scope.albumNameArray.push(user);
                    })
                    var data = {
                        "SmsMailStudentDetails": $scope.albumNameArray,
                        "SmsMailText": Text
                    };
                    apiService.create("ReportProspectus/SendSms", data)
                    $scope.$apply();
                    $scope.PostDataResponse = data;
                    swal("SMS Sent Successfully");
                }
                else{
                    swal("SMS Sending Canceled");
                }
            });
            }

         
           

          
      
        //MAIL sending function
        $scope.SendMAIL = function (Text) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Send MAIL?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
             function (isConfirm) {
                 if (isConfirm) {
                     $scope.albumNameArray = [];
                     angular.forEach($scope.reportdetails, function (user) {
                         if (!!user.selected) $scope.albumNameArray.push(user);
                     })

                     var data = {
                         "SmsMailStudentDetails": $scope.albumNameArray,
                         "SmsMailText": Text
                     };
                     apiService.create("ReportProspectus/SendMail", data)
                     $scope.$apply();
                     $scope.PostDataResponse = data;
                     confirm("Do You want to send MAIL");
                     swal('MAIL Sent Successfully');
                     $scope.saved = "MAIL Sent Successfully";
                 }
                 else {
                     swal("MAIL Sending Canceled");
                 }
             });
        }
       



        //Searching in the Grid View
        $scope.searchprospectus = function (enq) {

            if ($scope.enq.yearwiseorbtwdates === "yearwise") {
                $scope.enq.FromDate = "";
                $scope.enq.ToDate = "";
                $scope.enq.ASMAY = $scope.enq.ASMAY;
            }
            else if ($scope.enq.yearwiseorbtwdates === "btwdates") {
                $scope.enq.FromDate = $scope.enq.FromDate;
                $scope.enq.ToDate = $scope.enq.ToDate;
                $scope.enq.ASMAY = 0;
            }

        
            var data = {
                "MI_Id": 2,
                "ASMAY_Id": $scope.enq.ASMAY,
                "From_Date": $scope.enq.FromDate,
                "To_Date": $scope.enq.ToDate,
                searchString: $scope.enq.searchtext,
                searchType: $scope.enq.selectdropdown,

            }
            
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ReportProspectus/searchprospectus", data).
            then(function (promise) {
                if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {
                    
                    $scope.IsHiddendown = true;
                    $scope.reportdetails = promise.searchstudentDetails;
                    $scope.presentCountgrid = promise.searchstudentDetails.length;
                    $scope.pagination = true;

                }
                else {
                    swal('No Records found to display..!');
                  
                }
            });
        }
        

        //On selecting all option in the grid view drop down list
        $scope.all_func = function () {
            if ($scope.enq.selectdropdown == "all") {
                $scope.enq.searchtext = "";
                $scope.searchprospectus();
            }
        }


        //Cancel function
        $scope.cancel = function () {
            $state.reload();
            //$scope.enq.ASMAY = "";
            //$scope.enq.FromDate = "";
            //$scope.enq.ToDate = "";
            //$scope.print_flag = false;
            //$scope.sms_flag = false;
            //$scope.mail_flag = false;
            //$scope.export_flag = false;
            //$scope.IsHiddendown = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
         
        }

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });
    }

})();