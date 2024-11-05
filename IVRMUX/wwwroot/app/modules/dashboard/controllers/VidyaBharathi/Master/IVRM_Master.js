
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_MasterController', IVRM_MasterController)

    IVRM_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function IVRM_MasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.currentPage8 = 1;
        $scope.currentPage9 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage8 = 10;
        $scope.itemsPerPage9 = 10;
        $scope.itemsPerPage10 = 20;
        $scope.currentPage2 = 1;
        $scope.currentPage10 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.itemsPerPage5 = 10;
        $scope.currentPage3 = 1;
        $scope.currentPage5 = 1;
        $scope.searchValuethree = "";
        $scope.searchValue = "";
        $scope.searchValuefive = "";
        $scope.searchValuetwo = "";
        $scope.searchValuethreer = "";
        $scope.searchten = "";
        $scope.searcheight = "";
     
        $scope.obj = {};
        $scope.searchnine = "";
        $scope.taluka = [];
        $scope.panchayatlistone = [];
        $scope.myTabIndex = 0;
       
        $scope.editflags = false;
        $scope.state = false;
        $scope.distict = false;
        $scope.talukr = false;
        $scope.panchya = false;
    
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("IVRM_Master_ViddyBharthi/getalldetails", pageid).then(function (promise) {
                $scope.Country = promise.country;
                $scope.countrylist = promise.countryDetails;
                $scope.taluka = promise.talukalist;
                $scope.talukalistdet = promise.talukalist;
                $scope.pranthlist = promise.pranthlist;
                $scope.disctictlist = promise.distictlist;
                $scope.statelist = promise.statelist;              
                $scope.presentCountgrid = $scope.Country.length;
                $scope.presentCountgridtwo = promise.statelist.length;             
                if ($scope.countrylist != null && $scope.countrylist.length > 0) {
                    for (var i = 0; i < $scope.countrylist.length; i++) {
                        if ($scope.countrylist[i].ivrmmC_Default == true) {
                            $scope.obj.IVRMMC_Idone = $scope.countrylist[i].ivrmmC_Id;
                            $scope.obj.ivrmmC_Idtwo = $scope.countrylist[i].ivrmmC_Id;
                            $scope.obj.ivrmmC_Idthree = $scope.countrylist[i].ivrmmC_Id;
                            $scope.obj.ivrmmC_Idfour = $scope.countrylist[i].ivrmmC_Id;

                            $scope.CountryState($scope.countrylist[i].ivrmmC_Id);


                            return;
                        }

                    }
                }
            })

        };
        $scope.saveddata1 = function () {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var IVRMMC_Id = 0;
                if ($scope.obj.IVRMMC_Id > 0) {
                    IVRMMC_Id = $scope.obj.IVRMMC_Id;
                }
                var data = {
                    "IVRMMC_CountryName": $scope.obj.IVRMMC_CountryName,
                    "IVRMMC_CountryCode": $scope.obj.IVRMMC_CountryCode,
                    "IVRMMC_MobileNoLength": $scope.obj.IVRMMC_MobileNoLength,
                    "IVRMMC_Currency": $scope.obj.IVRMMC_Currency,
                    "IVRMMC_CountryPhCode": $scope.obj.IVRMMC_CountryPhCode,
                    "IVRMMC_Nationality": $scope.obj.IVRMMC_Nationality,
                    "IVRMMC_Id": IVRMMC_Id
                }
                apiService.create("IVRM_Master_ViddyBharthi/savecountry", data).
                    then(function (promise) {
                        $scope.clear1();
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else if (promise.returnval == "notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Records  Already Exist !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else if (promise.returnval == "notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                    })


            }

        };
        $scope.saveddata2 = function () {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {

                var IVRMMS_Id = 0;
                if ($scope.obj.IVRMMS_Id > 0) {
                    IVRMMS_Id = $scope.obj.IVRMMS_Id;
                }
                var data = {
                    "IVRMMS_Name": $scope.obj.IVRMMS_Name,
                    "IVRMMS_Id": IVRMMS_Id,
                    "IVRMMS_Code": $scope.obj.IVRMMS_Code,
                    "IVRMMC_Id": $scope.obj.IVRMMC_Idone,

                };
                apiService.create("IVRM_Master_ViddyBharthi/savestate", data).
                    then(function (promise) {
                        $scope.clear2();
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Records  Already Exist !');
                        }
                        else if (promise.returnval == "update") {

                            swal('Records Updated Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else if (promise.returnval == "notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }


                    })


            }
            else {
                $scope.submitted2 = true;

            }
        };
        $scope.QDistctict = function (IVRMMD_Id) {
            $scope.talukalist = []; $scope.panchayatlist = [];
            $scope.obj.IVRMMT_Id = ""; $scope.obj.IVRMMT_IdOne = "";
            if ($scope.taluka != null && $scope.taluka.length > 0) {
                for (var i = 0; i < $scope.taluka.length; i++) {
                    if ($scope.taluka[i].ivrmmD_Id == IVRMMD_Id) {
                        $scope.talukalist.push({
                            ivrmmT_Name: $scope.taluka[i].ivrmmT_Name,
                            ivrmmT_Id: $scope.taluka[i].ivrmmT_Id,

                        })
                    }
                }
            }
        };
        $scope.clear1 = function () {
            $scope.search = ""; $scope.obj.IVRMMC_CountryName = ""; $scope.obj.IVRMMC_CountryCode = ""; $scope.obj.IVRMMC_Id = "";
            $scope.obj.IVRMMC_MobileNoLength = ""; $scope.obj.IVRMMC_Currency = ""; $scope.obj.IVRMMC_CountryPhCode = "";
            $scope.obj.IVRMMC_Nationality = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();


        };
        $scope.submitted1 = false;
      
        $scope.editcountry = function (user) {
            $scope.obj.IVRMMC_Id = user.ivrmmC_Id;
            $scope.obj.IVRMMC_CountryName = user.ivrmmC_CountryName;
            $scope.obj.IVRMMC_CountryCode = user.ivrmmC_CountryCode;
            $scope.obj.IVRMMC_MobileNoLength = user.ivrmmC_MobileNoLength;
            $scope.obj.IVRMMC_CountryPhCode = user.ivrmmC_CountryPhCode;
            $scope.obj.IVRMMC_Currency = user.ivrmmC_Currency;
            $scope.obj.IVRMMC_Nationality = user.ivrmmC_Nationality;

        };
        $scope.editstate = function (user) {
            $scope.obj.IVRMMS_Id = user.ivrmmS_Id;
            $scope.obj.IVRMMS_Name = user.ivrmmS_Name;
            $scope.obj.IVRMMS_Code = user.ivrmmS_Code;
            $scope.obj.IVRMMC_Idone = user.ivrmmC_Id;
            $scope.obj.IVRMMS_MaxScholarshipQuota = user.ivrmmS_MaxScholarshipQuota;
            $scope.obj.MaxScholarshipStateQuota = user.ivrmmS_MaxScholarshipQuota;
            $scope.obj.IVRMMS_AllowScholashipFlg = user.ivrmmS_AllowScholashipFlg;
            $scope.BindData();

        };
        $scope.clear2 = function () {
            $scope.obj.IVRMMS_Id = ""; $scope.obj.IVRMMS_Name = ""; $scope.obj.IVRMMS_Code = "";
            $scope.obj.IVRMMS_MaxScholarshipQuota = "";
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.searchValuethreer = "";
            $scope.obj.IVRMMC_Idone = "";
            $scope.BindData();
        };
        $scope.submitted2 = false;
       
        $scope.submitted3 = false;
        $scope.clear3 = function () {
            $scope.obj.IVRMMD_Name = ""; $scope.obj.IVRMMD_Id = ""; $scope.obj.IVRMMD_Code = "";
            $scope.obj.IVRMMS_Idone = ""; $scope.obj.ivrmmC_Idtwo = ""; $scope.obj.IVRMMD_MaxScholarshipQuota = "";
            $scope.submitted3 = false;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.searchValuethree = "";
            $scope.BindData();

        };
        $scope.saveddata3 = function () {
            $scope.submitted3 = true;

            if ($scope.myForm3.$valid) {
                var IVRMMD_Id = 0;
                if ($scope.obj.IVRMMD_Id > 0) {
                    IVRMMD_Id = $scope.obj.IVRMMD_Id;
                }
                var data = {
                    "IVRMMD_Name": $scope.obj.IVRMMD_Name,
                    "IVRMMD_Id": IVRMMD_Id,
                    "IVRMMD_Code": $scope.obj.IVRMMD_Code,
                    "IVRMMS_Id": $scope.obj.IVRMMS_Idone,

                };

                apiService.create("IVRM_Master_ViddyBharthi/saveDistrict", data).
                    then(function (promise) {
                        $scope.clear3();
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Records  Already Exist !');
                        }
                        else if (promise.returnval == "update") {

                            swal('Records Updated Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else if (promise.returnval == "notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }


                    })


            }
            else {
                $scope.submitted3 = true;

            }
        };
        $scope.clear4 = function () {
            $scope.obj.IVRMMT_Id = ""; $scope.obj.IVRMMT_Name = "";
            $scope.obj.IVRMMD_IdOne = ""; $scope.obj.IVRMMT_MaxScholarshipQuota = "";
            $scope.submitted4 = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.BindData();

        };
        $scope.submitted4 = false;
        $scope.saveddata4 = function () {
            $scope.submitted4 = true;
            if ($scope.myForm4.$valid) {
                var IVRMMT_Id = 0;
                if ($scope.obj.IVRMMT_Id > 0) {
                    IVRMMT_Id = $scope.obj.IVRMMT_Id;
                }
                var data = {
                    "IVRMMT_Name": $scope.obj.IVRMMT_Name,
                    "IVRMMT_Id": IVRMMT_Id,
                    "IVRMMD_Id": $scope.obj.IVRMMD_IdOne,

                };

                apiService.create("IVRM_Master_ViddyBharthi/savetaluka", data).
                    then(function (promise) {
                        $scope.clear4();
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;

                        }
                        else if (promise.returnval == "notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "duplicate") {
                            swal('Records  Already Exist !');
                        }
                        else if (promise.returnval == "update") {

                            swal('Records Updated Successfully !');

                            $scope.myTabIndex = $scope.myTabIndex + 1;
                        }
                        else if (promise.returnval == "notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }


                    })


            }
            else {
                $scope.submitted4 = true;

            }
        };
        //taluka
        $scope.clear5 = function () {
            $scope.obj.IVRMMPCT_Id = ""; $scope.obj.IVRMMPCT_Name = ""; $scope.obj.IVRMMT_IdOne = "";
            $scope.obj.IVRMMPCT_MaxScholarshipQuota = "";
            $scope.submitted5 = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
            $scope.searchValuefive = "";
            $scope.BindData();

        };
    
        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {

            return $scope.submitted3 || field.$dirty;
        };
        $scope.interacted4 = function (field) {

            return $scope.submitted4 || field.$dirty;
        };
        $scope.interacted8 = function (field) {
            return $scope.submitted8 || field.$dirty;
        }
        $scope.interacted9 = function (field) {

            return $scope.submitted9 || field.$dirty;
        };
        $scope.CountryState = function (obj) {
            $scope.statedetails = [];
            $scope.disctictlistone = [];
            $scope.talukalist = []; 
            $scope.obj.IVRMMS_Idone = "";
            $scope.obj.IVRMMS_Idtwo = "";
            $scope.obj.IVRMMD_IdOne = "";
            $scope.obj.IVRMMD_Idtwo = "";
            $scope.obj.IVRMMT_IdOne = "";
            var data = {
                "IVRMMC_Id": obj
            };
            apiService.create("IVRM_Master_ViddyBharthi/onchnagestate", data).
                then(function (promise) {
                    $scope.statedetails = promise.statedetails;

                })

        }
        
        $scope.Stateone = function (obj) {
            $scope.disctictlistone = []; $scope.IVRMMD_Idtwo = ""; $scope.panchayatlist = []; $scope.obj.IVRMMD_Idtwo = "";
            $scope.talukalist = [];
            if ($scope.disctictlist != null && $scope.disctictlist.length > 0) {
                for (var i = 0; i < $scope.disctictlist.length; i++) {
                    if ($scope.disctictlist[i].ivrmmS_Id == obj) {
                        $scope.disctictlistone.push({
                            ivrmmD_Name: $scope.disctictlist[i].ivrmmD_Name,
                            ivrmmD_Id: $scope.disctictlist[i].ivrmmD_Id,

                        })
                    }
                }
            }
        }
        //TO clear  data                       
        $scope.Deletedatastate = function (item, SweetAlert) {

            var data = {

                "IVRMMS_Id": item.ivrmmS_Id
            }
            var dystring = "";
            if (item.ivrmmS_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.ivrmmS_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Master_ViddyBharthi/deactivestate", data).
                            then(function (promise) {
                                if (promise.returnval == "activate") {
                                    swal("Record " + dystring + "d Successfully !");
                                }
                                else if (promise.returnval == "notactivate") {
                                    swal("Record Not  Active / Deactive  !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                // $state.reload();
                                $scope.clear2();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.editdistict = function (user) {
            $scope.obj.ivrmmC_Idtwo = ""; $scope.obj.IVRMMS_Idone = ""; $scope.obj.IVRMMD_Name = ""; $scope.obj.IVRMMD_Code = ""; $scope.obj.IVRMMD_MaxScholarshipQuota = "";
            $scope.obj.IVRMMD_Id = "";
            $scope.obj.ivrmmC_Idtwo = user.ivrmmC_Id;
            $scope.obj.IVRMMS_Idone = user.ivrmmS_Id;
            $scope.obj.IVRMMD_Name = user.ivrmmD_Name;
            $scope.obj.IVRMMD_Code = user.ivrmmD_Code;
            $scope.obj.IVRMMD_Id = user.ivrmmD_Id;
            $scope.obj.IVRMMD_MaxScholarshipQuota = user.ivrmmD_MaxScholarshipQuota;
            $scope.obj.IVRMMD_AllowScholashipFlg = user.ivrmmD_AllowScholashipFlg;
        }
        $scope.deactivedistict = function (item, SweetAlert) {

            var data = {

                "IVRMMD_Id": item.ivrmmD_Id
            }
            var dystring = "";
            if (item.ivrmmD_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.ivrmmD_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Master_ViddyBharthi/deactivedistict", data).
                            then(function (promise) {
                                if (promise.returnval == "activate") {
                                    swal("Record " + dystring + "d Successfully !");
                                }
                                else if (promise.returnval == "notactivate") {
                                    swal("Record Not  Active / Deactive  !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                //  $state.reload();
                                $scope.clear3();
                                $scope.BindData();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        //deactivedistict      
        $scope.Deletedatacountry = function (item, SweetAlert) {

            var data = {

                "IVRMMC_Id": item.ivrmmC_Id
            }
            var dystring = "";
            if (item.ivrmmC_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.ivrmmC_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Master_ViddyBharthi/deactivateCountry", data).
                            then(function (promise) {
                                if (promise.returnval == "activate") {
                                    swal("Record " + dystring + "d Successfully !");
                                }
                                else if (promise.returnval == "notactivate") {
                                    swal("Record Not  Active / Deactive  !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
      
       
        //pnachyta
       
        $scope.edittaluka = function (user) {
            $scope.obj.ivrmmC_Idthree = user.ivrmmC_Id;
            $scope.obj.IVRMMS_Idtwo = user.ivrmmS_Id;
            $scope.Stateone(user.ivrmmS_Id);
            $scope.obj.IVRMMD_IdOne = user.ivrmmD_Id;
            $scope.obj.IVRMMT_Name = user.ivrmmT_Name;
            $scope.obj.IVRMMT_MaxScholarshipQuota = user.ivrmmT_MaxScholarshipQuota;
            $scope.obj.IVRMMT_Id = user.ivrmmT_Id;

            $scope.obj.IVRMMT_AllowScholashipFlg = user.ivrmmT_AllowScholashipFlg;


        }
        //deactivetaluka
        $scope.deactivetaluka = function (item, SweetAlert) {

            var data = {

                "IVRMMT_Id": item.ivrmmT_Id
            }
            var dystring = "";
            if (item.ivrmmT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.ivrmmT_ActiveFlag == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("IVRM_Master_ViddyBharthi/deactivetaluka", data).
                            then(function (promise) {

                                if (promise.returnval == "activate") {
                                    swal("Record " + dystring + "d Successfully !");
                                }
                                else if (promise.returnval == "notactivate") {
                                    swal("Record Not  Active / Deactive  !");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $scope.BindData();

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        //StateValidation
      
      
        //prantha
        $scope.submitted8 = false;
        
       
        //clear9
       
      
        //saveddata9
       
        
       
      
    }

})();