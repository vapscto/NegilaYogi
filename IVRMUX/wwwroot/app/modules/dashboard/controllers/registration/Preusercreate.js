(function () {
    'use strict';
    angular
        .module('app')
        .controller('PreusercreateController', PreusercreateController)

    PreusercreateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'Excel', 'superCache', '$timeout']
    function PreusercreateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, Excel, superCache, $timeout) {


        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.showgrid = false;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;

        //userid = configsettings[0].user;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        }
       
        $scope.coptyright = copty;
        $scope.sortKey = "User_Name";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;


        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.pagination = true;
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
            return ($filter('date')(obj.Created_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (obj.Email_Id).indexOf($scope.searchValue) >= 0 || (obj.Mob_No).indexOf($scope.searchValue) >= 0;
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

        };
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
        //};


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
        $scope.toggleAll = function () {

            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
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
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.reportdetails.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

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
            apiService.getURI("RegistrationReport/get_intial_data", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.fillyear;
                    $scope.classlist = promise.fillclass;
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




        //Date Compare function
        $scope.checkErr = function (FromDate, ToDate) {

            $scope.errMessage = '';
            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.IsHiddendown = false;
                $scope.errMessage = 'To Date should be greater than from date';

                return false;
            }

        };

        //Report showing function
        $scope.ShowReport = function () {
            //Form validation
            var data = {
                "MI_Id": 2,
            };
            apiService.create("RegistrationReport/Getdetailsforpre", data).
                then(function (promise) {
                    if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {
                        $scope.reportdetails = promise.searchstudentDetails;
                        $scope.showgrid = true;
                        $scope.presentCountgrid = promise.searchstudentDetails.length;
                    }
                    else {
                        swal("No Records Found")
                        $scope.showgrid = false;
                        $scope.pagination = false;
                        $scope.print_flag = false;
                        $scope.sms_flag = false;
                        $scope.export_flag = false;
                    }
                })
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
                        apiService.create("RegistrationReport/SendSms", data)
                        $scope.$apply();
                        $scope.PostDataResponse = data;
                        swal("SMS Sent Successfully");
                    }
                    else {
                        swal("SMS Sending Canceled");
                    }
                });
        }

        $scope.avtivedeactive = function (flg) {
            $scope.flg = flg;
            $scope.albumNameArray = [];
            angular.forEach($scope.printdatatable, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray.push(user);
                }
            })
            if ($scope.albumNameArray.length > 0) {
                var datalist =
                {
                    data_array: $scope.albumNameArray,
                    "count": flg
                }
                apiService.create("RegistrationReport/avtivedeactive", datalist).then(function (promise) {
                    //if (promise.success == "success") {
                        if ($scope.flg == 1) {
                            swal('Activated  Successfully...!', 'success');                            
                        }
                        else {
                            swal('De-Activated  Successfully...!', 'success');
                        }
                        $state.reload();
                    //}
                    //else {
                    //    swal('Failed to Active/De-activate..!', 'Failure');
                    //    return;
                    //}
                });
            }
        }

        $scope.smssend = function () {
            $scope.albumNameArray = [];
            angular.forEach($scope.printdatatable, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray.push(user);
                }
            })
            if ($scope.albumNameArray.length > 0) {
                var datalist =
                {
                    data_array: $scope.albumNameArray
                }
                apiService.create("RegistrationReport/smssend", datalist).then(function (promise) {
                    if (promise.success == "success") {
                        swal('SMS Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send SMS..!', 'Failure');
                        return;
                    }
                });
            }
        }

        $scope.emailsend = function () {

            //if ($scope.myFormSM.$valid)
            //{
            $scope.albumNameArray1 = [];
            angular.forEach($scope.printdatatable, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray1.push(user);
                }
            })
            if ($scope.albumNameArray1.length > 0) {
                var datalist =
                {
                    data_array: $scope.albumNameArray1
                }
                apiService.create("RegistrationReport/emailsend", datalist).then(function (promise) {

                    if (promise.success == "success") {
                        swal('Email Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send Email..!', 'Failure');
                        return;
                    }
                });
            }
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
                        apiService.create("RegistrationReport/SendMail", data)
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
            apiService.create("RegistrationReport/searchprospectus", data).
                then(function (promise) {
                    if (promise.searchstudentDetails != null && promise.searchstudentDetails.length > 0) {

                        $scope.IsHiddendown = false;
                        $scope.reportdetails = promise.searchstudentDetails;
                        $scope.presentCountgrid = promise.searchstudentDetails.length;


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




        $scope.submitted = false;
        $scope.savestudentdata = function (asd) {

            if ($scope.myForm.$valid) {

                var vm = this;
                var formData = new FormData();
                var onlineprofilepicpath = "";
                var password = "Password@123";
                $scope.moboto = false;
                $scope.emailboto = false;
                formData.append("onlinepreadminfilepath", onlineprofilepicpath);

                formData.append("Name", $scope.reg.PASR_FirstName);
                formData.append("Email_id", $scope.reg.PASR_emailId);
                formData.append("Mobileno", $scope.reg.PASR_MobileNo);
                formData.append("Username", $scope.reg.username);
                formData.append("Password", password);
                formData.append("Otpmobl", $scope.moboto);
                formData.append("Otpemail", $scope.emailboto);
                formData.append("Special", "Special");
                //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);

                $http.post("/api/Login/Regis", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).then(function (response) {
                        if (response.data != "" && response.data != "created") {
                            swal(response.data);
                        }
                        else if (response.data == "created") {
                            swal("Account created successfully");
                            $state.reload();
                        }
                        else {
                            swal(response.data);
                            $state.reload();
                        }

                    })
            }

            else {
                $scope.submitted = true;
            }

        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

    }



})();