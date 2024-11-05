
(function () {
    'use strict';
    angular
.module('app')
.controller('TransportApprovedController', TransportApprovedController)

    TransportApprovedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function TransportApprovedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ssm = false;
        $scope.emials = false;
        $scope.rejectlist = false;
        $scope.firstreject = true;
        $scope.sortKey = "trmL_Id";   //set the sortKey to the param passed
        $scope.sortReverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "trmL_Id";   //set the sortKey to the param passed
        $scope.sortReverse1 = true; //if true make it false and vice versa


        $scope.obj = {};
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgnameee = logopath;

        $scope.page1 = "pag1";
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;


        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;

        $scope.currentPage111 = 1;
        $scope.itemsPerPage111 = 10;



        $scope.listshow = false;
        $scope.showww = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("TransportApproved/getdata", pageid).
        then(function (promise) {
            if (promise != null) {

                if (promise.logoheader.length > 0) {
                    $scope.imgname = promise.logoheader[0].logopath;
                }
                else {
                    $scope.imgname = logopath;
                }
                $scope.getaccyear = promise.getaccyear;
                $scope.getclass = promise.getclass;
                $scope.getdetails = promise.getdetails;
                $scope.presentCountgrid1 = $scope.getdetails.length;

                angular.forEach($scope.getdetails, function (rr) {

                    if (rr.applicationno != null && rr.applicationno != "" && rr.applicationno != undefined) {
                        rr.applicationno = parseInt(rr.applicationno);
                    }
                })


                //added Praveen
                $scope.ASMAY_IDS = promise.asmaY_Id;
                $scope.picsesslist = promise.picsesslist;
                $scope.drpsesslist = promise.drpsesslist;
                //end Praveen
                $scope.routename = promise.routename;
                if ($scope.getdetails.length > 0) {
                    $scope.showww = true;
                }
                else {
                    $scope.showww = true;
                }
            }
        })
        }

        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.interacted = function (field) {
            debugger;
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.gridaconchange = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_IDS
            }
            apiService.create("TransportApproved/gridaconchange", data).then(function (promise) {
                if (promise.getdetails.length > 0) {
                    $scope.showww = true;
                    $scope.getdetails = promise.getdetails;
                    angular.forEach($scope.getdetails, function (rr) {

                        if (rr.applicationno != null && rr.applicationno != "" && rr.applicationno != undefined) {
                            rr.applicationno = parseInt(rr.applicationno);
                        }
                    })
                    $scope.presentCountgrid1 = $scope.getdetails.length;
                } else {
                    swal("No Records Found")
                    $scope.showww = true;
                }
            })
        }

        //---Search--//
        $scope.searchdetails = function (obj) {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "TRMR_Id": $scope.obj.trmR_Id,
                    "RegularNew": $scope.obj.newreg,
                }
                apiService.create("TransportApproved/searchdetails", data).then(function (promise) {
                    if (promise.getalldetails.length > 0) {
                        $scope.locationdetails = promise.getalldetails
                        $scope.listshow = true;
                        $scope.presentCountgrid = $scope.locationdetails.length;


                        angular.forEach($scope.locationdetails, function (xx) {
                            if (xx.applicationno != null && xx.applicationno != "" && xx.applicationno != undefined) {
                                xx.applicationno = parseInt(xx.applicationno);
                            }
                           
                            xx.sesslist_list_p = $scope.picsesslist;
                            xx.sesslist_list_d = $scope.drpsesslist;
                            xx.PickUp_Session = '';
                            xx.Drop_Session = '';
                        })

                        console.log($scope.locationdetails);
                    } else {
                        swal("No Records Found")
                        $scope.listshow = false;
                    }
                })
            }
            else {
                $scope.submitted1 = true;
            }
        }


        $scope.showmodaldetails = function (astA_Id, amsT_Id) {
            $scope.buspassdatalst = [];
            var data = {
                "AMST_Id": amsT_Id,
                "ASTA_Id": astA_Id
            }
            apiService.create("TransportApproved/showmodaldetails", data).
               then(function (promise) {
                   //
                   //$scope.getdate = new Date();
                   //$scope.buspassdatalst = promise.studentdetails;
                   //$scope.appno = $scope.buspassdatalst[0].appno;
                   //$scope.obj.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   //$scope.obj.AMST_AdmNo = $scope.buspassdatalst[0].AMST_AdmNo;
                   //$scope.obj.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   //$scope.obj.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   //$scope.obj.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   //$scope.obj.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   //$scope.obj.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   //$scope.obj.fuyear = $scope.buspassdatalst[0].fuyear;

                   //$scope.obj.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   //$scope.obj.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   //$scope.obj.amsT_FatherMobleNo = $scope.buspassdatalst[0].AMST_FatherMobleNo;

                   //$scope.obj.amsT_MotherMobileNo = $scope.buspassdatalst[0].AMST_MotherMobileNo;
                   //$scope.obj.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   ////------------Address
                   //$scope.obj.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   //$scope.obj.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   //$scope.obj.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   //$scope.obj.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   //$scope.obj.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   //$scope.obj.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;

                   $scope.getdate = new Date();
                   $scope.buspassdatalst = promise.studentdetails;
                   $scope.appno = $scope.buspassdatalst[0].appno;
                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   $scope.pickuprouteno = $scope.buspassdatalst[0].pickuprouteno;
                   $scope.ASMAY_Year = $scope.buspassdatalst[0].ASMAY_Year;
                   $scope.AMST_AdmNo = $scope.buspassdatalst[0].AMST_AdmNo;
                   $scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTA_Landmark;
                   $scope.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   $scope.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   $scope.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   $scope.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   $scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   $scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                   $scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   $scope.fuyear = $scope.buspassdatalst[0].fuyear;

                   $scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   $scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                   $scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   $scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTA_FatherMobileNo;

                   $scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTA_MotherMobileNo;
                   $scope.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   //------------Address
                   $scope.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   $scope.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   $scope.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   $scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   $scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   $scope.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;
                   $scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTA_Regnew;
                   $scope.amsT_Office = $scope.buspassdatalst[0].ASTA_Phoneoff;
                   $scope.amsT_Res = $scope.buspassdatalst[0].ASTA_PhoneRes;
                   $scope.getdate = $scope.buspassdatalst[0].ASTA_ApplicationDate;
                   var e1 = angular.element(document.getElementById("test"));
                   $compile(e1.html(promise.htmldata))(($scope));
                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   $('#blahnewF').attr('src', $scope.buspassdatalst[0].ANST_FatherPhoto);
                   $('#blahnewM').attr('src', $scope.buspassdatalst[0].ANST_MotherPhoto);
               })
        }

        $scope.printbusspassbbhs = function () {
            
            var innerContents = document.getElementById("BBHSBUSSFORM").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BBHS/BBHSBUSSFORM/BBHSBUSSFORMPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BGHSAPP = function () {
            
            var innerContents = document.getElementById("BGHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                       '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                 '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //$scope.sms1 = false;
        //$scope.email1 = false;
        //Save Approved List and rejected list
        $scope.saveapproved = function (obj) {
        //    debugger;
            if ($scope.printdatatable.length > 0) {

                console.log($scope.printdatatable);
                //angular.forEach($scope.printdatatable, function (rr) {

                //    if ($scope.all2 == true) {

                //        if (rr.remarks1 == null && rr.remarks1 == undefined) {
                //            rr.remarks1 = '';
                //        }
                //        if (rr.studentremarkemail1 == null && rr.studentremarkemail1 == undefined) {
                //            rr.studentremarkemail1 = '';
                //        }

                //    }
                 
                //})
                $scope.submitted = false;
                $scope.emailclick = [];
                $scope.smsclick = [];
                angular.forEach($scope.locationdetails, function (itm) {
                    if (itm.chkemail == true) {
                        $scope.emailclick.push({
                            AMST_Id: itm.amsT_Id,
                            studentname: itm.studentname,
                            studentremarkemail1: itm.studentremarkemail1,
                            ASTA_Id: itm.ASTA_Id
                        });
                    }
                });
                angular.forEach($scope.locationdetails, function (itm) {
                    if (itm.chksms == true) {
                        $scope.smsclick.push({
                            AMST_Id: itm.amsT_Id,
                            studentname: itm.studentname,
                            remarks1: itm.remarks1,
                            ASTA_Id: itm.ASTA_Id
                            //  studentname: itm.studentName
                        });
                    }
                });

                if ($scope.myForm1.$valid) {
                    var data = {
                         data_array: $scope.printdatatable,
                        "Temp_Save_List": $scope.printdatatable,
                        "ASMAY_Id": $scope.obj.asmaY_Id,
                        "ASMCL_Id": $scope.obj.asmcL_Id,
                        "Flag": "A",
                        "smsclick": $scope.smsclick,
                        "emailclick": $scope.emailclick
                    }
                    console.log($scope.printdatatable);
                    apiService.create("TransportApproved/savelist", data).then(function (promise) {
                        if (promise != null) {
                            swal(promise.message);
                            $state.reload();
                        }
                    })

                  
                }
                else {
                    $scope.submitted == true;
                }

            } else {
                swal("Select Student List")
            }
        }


        $scope.sms = false;
        $scope.email = false;
        //Save Approved List and rejected list
        $scope.saveapprovedreject = function (obj) {
            if ($scope.printdatatable.length > 0) {
                console.log($scope.printdatatable);
                angular.forEach($scope.printdatatable, function (rr) {

                    if (rr.remarks == null && rr.remarks == undefined) {
                        rr.remarks = '';
                    }
                    if (rr.studentremarkemail == null && rr.studentremarkemail == undefined) {
                        rr.studentremarkemail = '';
                    }

                })


                var data = {
                    data_array:$scope.printdatatable,
                    "Temp_Save_List": $scope.printdatatable,
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "Flag": "R",
                    "smscheck": $scope.sms,
                    "emailcheck": $scope.email
                }
                apiService.create("TransportApproved/savelist", data).then(function (promise) {
                    if (promise != null) {
                        swal(promise.message);
                        $state.reload();
                    }
                })
            } else {
                swal("Select Student List")
            }
        }


       // $scope.locationdetails = [];

        $scope.smsAll = function () {
        
            if ($scope.sms2 == true) {

                angular.forEach($scope.locationdetails, function (itm) {
                    itm.chksms = true;
                });
            }
            else {

                angular.forEach($scope.locationdetails, function (itm) {
                    itm.chksms = false;
                });
            }
               
        }


        $scope.email2All = function () {
          
            if ($scope.email2 == true) {

                angular.forEach($scope.locationdetails, function (itm) {
                    itm.chkemail = true;
                });
            }
            else {

                angular.forEach($scope.locationdetails, function (itm) {
                    itm.chkemail = false;
                });
            }
         
        }


        $scope.printdatatable = [];

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.locationdetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                  
                    $scope.printdatatable.push(itm);

                   
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.saveapprovedrejectlist=function()
        {
            if ($scope.printdatatable.length > 0) {
                $scope.rejectlist = true;
                $scope.firstreject = false;
            }
            else {
                swal("Select Student List For Rejection!")
            }
        }

        $scope.reject = false;
        $scope.optionToggled1 = function (SelectedStudentRecord, index) {
            $scope.reject = false;
            var selectcount = 0;
            angular.forEach($scope.getdetails, function (dd) {

                if (dd.selected) {
                    //if (dd.astA_ApplStatus == 'Rejected') {
                    //    dd.selected = false;
                    //    swal('Status Already Rejected')
                    //} else {
                        selectcount += 1;
                   // }
                   
                }
              
            })
            if (selectcount > 0) {
                $scope.reject = true;
            }
        }

        //$scope.smsToggled = function (SelectedStudentRecord, index) {

        //    $scope.sms2 = $scope.locationdetails.every(function (itm) { return itm.selected; });
        //    if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
        //        SelectedStudentRecord.remarks = '';
        //        SelectedStudentRecord.studentremarkemail = '';
        //        $scope.printdatatable.push(SelectedStudentRecord);
                
        //        //if ($scope.sms2 == true) {


        //        //    if ($scope.sms1 == true) {
        //        //        $scope.ssm1 = true;
        //        //    }
        //        //    else {
        //        //        $scope.ssm1 = false;
        //        //    }
        //        //}

        //    }
        //    else {
        //        $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 2);
        //    }
        //}


     

        $scope.saveapprovedrejectlist111 = function () {
            debugger; 
            $scope.printdatatable1 = [];
            angular.forEach($scope.getdetails, function (dd) {

                if (dd.selected) {
                    $scope.printdatatable1.push(dd)
                }
            })
                var data = {
                    "Temp_Save_List": $scope.printdatatable1,
                }
                debugger; 
            apiService.create("TransportApproved/editapprove", data).then(function (promise) {
                debugger;
                    if (promise != null) {
                        swal(promise.message);
                        $state.reload();
                    }
                })
            
        }


        $scope.CancelRejection = function () {
            debugger;
            $scope.printdatatable1 = [];
            angular.forEach($scope.getdetails, function (dd) {

                if (dd.selected) {
                    $scope.printdatatable1.push(dd)
                }
            })
            var data = {
                "Temp_Save_List": $scope.printdatatable1,
            }
            debugger;
            apiService.create("TransportApproved/CancelRejection", data).then(function (promise) {
                debugger;
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })

        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all2 = $scope.locationdetails.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                SelectedStudentRecord.remarks = '';
                SelectedStudentRecord.studentremarkemail = '';
                $scope.printdatatable.push(SelectedStudentRecord);
                
            
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }
       

        $scope.smssending=function()
        {
            if ($scope.sms == true)
            {
                $scope.ssm = true;
            }
            else {
                $scope.ssm = false;
            }
        }


        $scope.emailsending = function () {
            if ($scope.email == true) {
                $scope.emials = true;
            }
            else {
                $scope.emials = false;
            }
        }

        //$scope.smssending1 = function () {

        //    if ($scope.all2 == true) {

              
        //        if ($scope.sms1 == true) {
        //            $scope.ssm1 = true;
        //        }
        //        else {
        //            $scope.ssm1 = false;
        //        }
        //    }
           
        //}


        //$scope.emailsending1 = function () {

        //    if ($scope.all2 == true) {
        //        if ($scope.email1 == true) {
        //            $scope.emials1 = true;
        //        }

        //    } else {

                
        //            $scope.emials1 = false;
               
        //    }
           
        //}

        $scope.searchValue = '';
        //$scope.filterValue = function (obj) {
        //    return (angular.lowercase(obj.studentname)).indexOf($scope.searchValue) >= 0 ||
        //        (angular.lowercase(obj.applicationno)).indexOf($scope.searchValue) >= 0 ||
        //          (angular.lowercase(obj.astA_ApplStatus)).indexOf($scope.searchValue) >= 0 ||

        //         (angular.lowercase(obj.amsT_AdmNo)).indexOf($scope.searchValue) >= 0 ||
        //         (angular.lowercase(obj.asmcL_ClassName)).indexOf($scope.searchValue) >= 0 ||

        //        (angular.lowercase(obj.areaname)).indexOf($scope.searchValue) >= 0 ||
        //         (angular.lowercase(obj.pickuproute)).indexOf($scope.searchValue) >= 0 ||
        //          (angular.lowercase(obj.pickuplocation)).indexOf($scope.searchValue) >= 0 ||
        //        (angular.lowercase(obj.drouproute)).indexOf($scope.searchValue) >= 0 ||
        //        (angular.lowercase(obj.drouplocation)).indexOf($scope.searchValue) >= 0 ||
        //        (angular.lowercase(obj.neworreguular)).indexOf($scope.searchValue) >= 0
        //}

        $scope.searchValue1 = '';
        //$scope.filterValue12 = function (obj) {
        //    return (angular.lowercase(obj.studentname)).indexOf($scope.searchValue1) >= 0 ||
        //        (angular.lowercase(obj.applicationno)).indexOf($scope.searchValue1) >= 0 ||
        //          (angular.lowercase(obj.astA_ApplStatus)).indexOf($scope.searchValue1) >= 0 ||
        //        (angular.lowercase(obj.areaname)).indexOf($scope.searchValue1) >= 0
        //}


        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //--Sorting--//     
        $scope.sort1 = function (key) {
            $scope.sortReverse1 = ($scope.sortKey1 == key) ? !$scope.sortReverse1 : $scope.sortReverse1;
            $scope.sortKey1 = key;
        }

        $scope.cancel = function () {
            $state.reload();
        }
    };
})();


