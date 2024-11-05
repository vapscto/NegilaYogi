

(function () {
    'use strict';
    angular
.module('app')
.controller('enquiryreportcontroller', enquiryreportcontroller)

    //enquiryreportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter','superCache']
    //function enquiryreportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        enquiryreportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
        function enquiryreportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Date:09-02-2017---Searching, sorting ,pagination and collapse functionality///

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.pasE_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || JSON.stringify(obj.pasE_MobileNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.pasP_Phone).indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pasE_FirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_MiddleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_LastName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_emailid)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_EnquiryNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_Address1)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_Address2)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_Address3)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_EnquiryDetails)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

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

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.sortKey = "Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        //$scope.pagination = false;
        $scope.validate = false;
        $scope.sms_flag = false;
        $scope.mail_flag = false;
        $scope.export_flag = false;
        $scope.print_flag = false;
        var pageid = 2;

        $scope.errMessage_Year = 'Select Academic Year';
        $scope.errMessage_From_Date = 'Select From Date from the Calendar';
        $scope.errMessage_To_Date = 'Select To Date from the Calendar';


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


   

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return ($filter('date')(obj.pasE_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (angular.lowercase(obj.pasE_FirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_MiddleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_LastName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_emailid)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_EnquiryNo)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_Address1)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || JSON.stringify(obj.pasE_MobileNo).indexOf($scope.searchValue) >= 0 || (angular.lowercase(obj.pasE_Address2)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_Address3)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.pasE_EnquiryDetails)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }


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





        $scope.loaddata = function () {
            apiService.getURI("EnquiryReport/getdetails", pageid).
        then(function (promise) {
            
            $scope.yearlst = promise.fillyear;
        })
        }


        $scope.propertyName = 'pasE_FirstName';

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }



        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();

            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.IsHiddendown = false;
                $scope.errMessage = 'To Date should be greater than from date';
                //$scope.submitted = false;

                return false;

            }


        };


        //For date validation
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;


        $scope.all_func = function () {
            if ($scope.enq.selectdropdown == "all") {
                $scope.enq.searchtext = "";
                //$scope.text_flag = false;
                $scope.searchenquiry();
            }
        }





        $scope.ShowReport = function (enq) {
            //swal("hi");
            
           

            if ($scope.myForm.$valid) {

                if ($scope.enq.yearwiseorbtwdates == "yearwise") {
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
                    
                    //"ASMAY_Id": 10,
                    "From_Date": $scope.enq.FromDate,
                    "To_Date": $scope.enq.ToDate,
                    "ASMAY_Id": $scope.enq.ASMAY,

                };

                apiService.create("enquiryreport/searchdata", data).
       then(function (promise) {


           if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {

               $scope.pagination = true;
               $scope.IsHiddendown = true;
               $scope.export_flag = true;
               $scope.print_flag = true;
               $scope.sms_flag = true;
               $scope.mail_flag = true;

               $scope.reportdetails = promise.searchstudentDetails;
               $scope.presentCountgrid = promise.searchstudentDetails.length;
           }
           else {

               swal("No records Found");
               $scope.pagination = false;
               $scope.IsHiddendown = false;
               $scope.sms_flag = false;
               $scope.mail_flag = false;
               $scope.export_flag = false;
               $scope.print_flag = false;
               //$state.reload();

           }

       })
            }
            else {
                $scope.submitted = true;

            }

        };

        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all2;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all2 == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all2;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all2 == true) {
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
                $scope.all2 = $scope.reportdetails.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all2 = $scope.filterValue1.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        }



        $scope.SendMSG = function (Text) {
            
            $scope.albumNameArray = [];
            angular.forEach($scope.reportdetails, function (user) {
                if (!!user.selected) $scope.albumNameArray.push(user);
            })

            if ($scope.albumNameArray.length > 0)
            {

                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
             function (isConfirm) {
                 if (isConfirm) {


                     var data = {
                         "SmsMailStudentDetails": $scope.albumNameArray,
                         "SmsMailText": Text
                     };
                     apiService.create("enquiryreport/SendSms", data).then(function (promise) {
                         
                         swal('SMS Sent Successfully');
                         $state.reload();
                     })
                    
                 }
                 else {
                     swal("SMS Sending Canceled");
                 }
             });
            }
            else{
                    swal("Kindly select atleast one Student..!!");
                    return;
                }            
        }

        $scope.SendMAIL = function (Text) {

            $scope.albumNameArray = [];
            angular.forEach($scope.reportdetails, function (user) {
                if (!!user.selected) $scope.albumNameArray.push(user);
            })
            if ($scope.albumNameArray.length > 0)
            {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send MAIL?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
             function (isConfirm) {
                 if (isConfirm) {
                     var data = {
                         SmsMailStudentDetails: $scope.albumNameArray,
                         "SmsMailText": Text
                     };
                     apiService.create("enquiryreport/SendMail", data).then(function (promise) {

                         swal('MAIL Sent Successfully');
                         $state.reload();
                     })
                    
                 }
                 else {
                     swal("MAIL Sending Cancelled");
                 }
             });
            }
            else {
                swal("Kindly select atleast one Student..!!");
                return;
            }
        }




        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;


            }
        };




        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });


        //$scope.a = [];
        $scope.searchenquiry = function (enq) {
            if ($scope.enq.yearwiseorbtwdates == "yearwise") {
                $scope.enq.FromDate = '';
                $scope.enq.ToDate = '';
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
                //"ASMAY_Id": 10,
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
            apiService.create("enquiryreport/searchenquiry", data).
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

        $scope.frdatetodate = true;
        $scope.accyear = false;

        $scope.onclickloaddata = function () {
            if ($scope.enq.yearwiseorbtwdates === 'yearwise') {
                $scope.accyear = false;
                $scope.frdatetodate = true;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.IsHiddendown = false;
                // $scope.errMessage_Year = "";
                $scope.enq.ASMAY = "";
                $scope.submitted = false;
                // $state.reload();
                // $scope.errMessage_Year = '';

            }
            else if ($scope.enq.yearwiseorbtwdates === 'btwdates') {
                $scope.accyear = true;
                $scope.frdatetodate = false;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                //$scope.errMessage_From_Date = "";
                //$scope.errMessage_To_Date = "";
                $scope.IsHiddendown = false;
                $scope.submitted = false;

            }
        };
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
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        }



    }

})();