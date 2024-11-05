
(function () {
    'use strict';
    angular
.module('app').controller('CLGTransportApproveController', CLGTransportApproveController)
    CLGTransportApproveController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function CLGTransportApproveController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

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
            apiService.getURI("CLGTransportApprove/getdata", pageid).then(function (promise) {
            if (promise != null) {

                if (promise.logoheader.length > 0) {
                    $scope.imgname = promise.logoheader[0].logopath;
                }
                else {
                    $scope.imgname = logopath;
                }
                $scope.getaccyear = promise.getaccyear;
                $scope.getclass = promise.getcourse;
                $scope.getdetails = promise.getdetails;
                $scope.presentCountgrid1 = $scope.getdetails.length;
                $scope.templocation = [];
                $scope.templocation = promise.getalldetails;
                if ($scope.templocation != null && $scope.templocation.length > 0) {

                }
                else {

                }
                $scope.locationdetails = promise.getalldetails;               
                $scope.presentCountgrid = $scope.locationdetails.length;
                $scope.pickuproutename = promise.routename;
                $scope.Droproutename = promise.routename;
                //added Praveen
                $scope.ASMAY_IDS = promise.asmaY_Id;
                $scope.picsesslist = promise.picsesslist;
                $scope.drpsesslist = promise.drpsesslist;
                //end Praveen





                angular.forEach($scope.locationdetails, function (xx) {
                    if (xx.applicationno != null && xx.applicationno != "" && xx.applicationno != undefined) {
                        xx.applicationno = parseInt(xx.applicationno);
                    }

                    xx.sesslist_list_p = $scope.picsesslist;
                    xx.sesslist_list_d = $scope.drpsesslist;
                    xx.PickUp_Session = '';
                    xx.Drop_Session = '';
                })

                angular.forEach($scope.getdetails, function (rr) {
                    
                    if (rr.applicationno != null && rr.applicationno != "" && rr.applicationno != undefined) {
                        rr.applicationno = parseInt(rr.applicationno);
                    }
                })
                
                if ($scope.getdetails.length > 0) {
                    $scope.showww = true;
                }
                else {
                    $scope.showww = true;
                }
            }
        })
        }
        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.semisterlist = [];
            $scope.branchlist = [];
            $scope.semisterlist = [];
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTRNCommon/getbranch", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Course");
                    }
                })

        };
        $scope.get_semister = function () {
            $scope.ACMS_Id = '';
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTRNCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })
        };
        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.interacted = function (field) {
       
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.gridaconchange = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_IDS
            }
            apiService.create("CLGTransportApprove/gridaconchange", data).then(function (promise) {
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
        $scope.searchdetails = function () {

            var semlist = [];

            if ($scope.AMSE_Id==0) {
                semlist = $scope.semisterlist
            }
            else {
                angular.forEach($scope.semisterlist, function (dd) {
                    if ($scope.AMSE_Id == dd.amsE_Id) {
                        semlist.push(dd);
                    }

                })
            }
            


            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    AMSE_Idss: semlist,
                    "TRMR_Id": $scope.trmR_Id,
                    "RegularNew": $scope.newreg,
                }
                apiService.create("CLGTransportApprove/searchdetails", data).then(function (promise) {
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
            debugger;
            $scope.buspassdatalst = [];
            var data = {
                "AMCST_Id": amsT_Id,
                "ASTACO_Id": astA_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CLGTransportApprove/showmodaldetails", data).
               then(function (promise) {
                 

                   $scope.getdate = new Date();
                   $scope.buspassdatalst = promise.studentdetails;
                 
                   $scope.appno = $scope.buspassdatalst[0].appno;
                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   $scope.AMST_AdmNo = $scope.buspassdatalst[0].AMCST_AdmNo;
                   $scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTACO_Landmark;
                   $scope.amcsT_FirstName = $scope.buspassdatalst[0].stuname;
                   $scope.amcsT_FatherName = $scope.buspassdatalst[0].AMCST_FatherName;
                   $scope.amsE_SEMName = $scope.buspassdatalst[0].FutureSem;
                   $scope.amcO_CourseName = $scope.buspassdatalst[0].AMCO_CourseName;
                   $scope.amB_BranchName = $scope.buspassdatalst[0].AMB_BranchName;
                   $scope.amcsT_BloodGroup = $scope.buspassdatalst[0].AMCST_BloodGroup;
                   $scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   $scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                   $scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   $scope.fuyear = $scope.buspassdatalst[0].fuyear;

                   $scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   $scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                   $scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   $scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTACO_PickupSMSMobileNo;

                   $scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTACO_DropSMSMobileNo;
                   $scope.amsT_emailId = $scope.buspassdatalst[0].AMCST_emailId;
                   //------------Address
                   $scope.amsT_PerStreet = $scope.buspassdatalst[0].AMCST_PerStreet;
                   $scope.amsT_PerArea = $scope.buspassdatalst[0].AMCST_PerArea;
                   $scope.amsT_PerCity = $scope.buspassdatalst[0].AMCST_PerCity;
                   $scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   $scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   $scope.amsT_PerPincode = $scope.buspassdatalst[0].AMCST_PerPincode;
                   $scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTACO_Regnew;
                   $scope.amsT_Office = $scope.buspassdatalst[0].ASTACO_Phoneoff;
                   $scope.amsT_Res = $scope.buspassdatalst[0].ASTACO_PhoneRes;
                   $scope.getdate = $scope.buspassdatalst[0].ASTACO_ApplicationDate;
                   debugger;
                   $scope.MI_Name = $scope.buspassdatalst[0].MI_Name;
                   $scope.IVRMMCT_Name = $scope.buspassdatalst[0].IVRMMCT_Name;
                   $scope.MI_Pincode = $scope.buspassdatalst[0].MI_Pincode;
                   $scope.MI_Address1 = $scope.buspassdatalst[0].MI_Address1;
                   var e1 = angular.element(document.getElementById("test"));
                   $compile(e1.html(promise.htmldata))(($scope));
                   $('#blahnew').attr('src', $scope.buspassdatalst[0].AMCST_StudentPhoto);
                   $('#blahnewF').attr('src', $scope.buspassdatalst[0].AMCST_FatherPhoto);
                   $('#blahnewM').attr('src', $scope.buspassdatalst[0].AMCST_MotherPhoto);
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

        //Save Approved List and rejected list
        $scope.saveapproved = function (obj) {
            debugger;
            if ($scope.printdatatable.length > 0) {
                $scope.submitted = false;
                if ($scope.myForm1.$valid) {

                    angular.forEach($scope.printdatatable, function (rr) {

                        if (rr.PickUp_Session=="") {
                            rr.PickUp_Session = 0;
                        }

                        if (rr.Drop_Session == "") {
                            rr.Drop_Session = 0;
                        }

                    })
                    
                    var data = {
                        "Temp_Save_List": $scope.printdatatable,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "Flag": "A"
                    }
                    console.log($scope.printdatatable);
                    apiService.create("CLGTransportApprove/savelist", data).then(function (promise) {
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
                    if (rr.Drop_Session=="") {
                        rr.Drop_Session = 0;
                    }
                    if (rr.PickUp_Session == "") {
                        rr.PickUp_Session = 0;
                    }

                })


                var data = {
                    data_array:$scope.printdatatable,
                    "Temp_Save_List": $scope.printdatatable,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "Flag": "R",
                    "smscheck": $scope.sms,
                    "emailcheck": $scope.email
                }
                apiService.create("CLGTransportApprove/savelist", data).then(function (promise) {
                    if (promise != null) {
                        swal(promise.message);
                        $state.reload();
                    }
                })
            } else {
                swal("Select Student List")
            }
        }



        $scope.printdatatable = [];

        $scope.toggleAll = function (all2) {
            var toggleStatus = all2;
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

        $scope.saveapprovedrejectlist111 = function () {
            debugger; 
            $scope.printdatatable1 = [];
            angular.forEach($scope.getdetails, function (dd) {

                if (dd.selected) {
                    $scope.printdatatable1.push({ AMCST_Id: dd.amcsT_Id, FASMAY_Id: dd.fasmaY_Id, ASTACO_Id: dd.astacO_Id})
                }
            })
                var data = {
                    "Temp_Save_List": $scope.printdatatable1,
                }
                debugger; 
            apiService.create("CLGTransportApprove/editapprove", data).then(function (promise) {
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
                    $scope.printdatatable1.push({ ASTACO_Id: dd.astacO_Id})
                }
            })
            var data = {
                "Temp_Save_List": $scope.printdatatable1,
            }
            debugger;
            apiService.create("CLGTransportApprove/CancelRejection", data).then(function (promise) {
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


