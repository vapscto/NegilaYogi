(function () {
    'use strict';
    angular
        .module('app')
        .controller('CirculationParameterController', CirculationParameterController)

    CirculationParameterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CirculationParameterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        //------sorting recod....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.issuertype1 = 'STUDENT';

        $scope.onclickloaddata = function () {
            $scope.Loaddata();
            //var data = {
            //    "LMC_CategoryName": $scope.book_Flag,
            //}

            //apiService.create("CirculationParameter/getdata", data).then(function (promise) {
            //    $scope.categlst = promise.categorylist;
            //    $scope.alldata = promise.alldata;
            //})

        };


        $scope.booktypedr = true;
        $scope.classdrdr = true;
        $scope.cempgrpdr = false;
        $scope.clgclassdrdr = false;
        $scope.schclgflag ="";

        $scope.book_Flag = "BP";
          //=====================Loaddata
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
  
        var pageid = 2;
        $scope.stf = false;
        $scope.std = false;

        $scope.transtypechange = function () {

            $scope.Loaddata();
        }
        $scope.Loaddata = function () {
          
           
            var data = {
                "LMC_CategoryName": $scope.book_Flag,
                "BOOKFLAG": $scope.book_Flag,
                "issuertype1": $scope.issuertype1,
            }
            apiService.create("CirculationParameter/getdetails", data).then(function (promise) {

                $scope.schclgflag = promise.mI_SchoolCollegeFlag;

                if ($scope.book_Flag == "BP" && $scope.issuertype1 == 'STUDENT') {

                    if ($scope.schclgflag=='S') {
                        $scope.classdrdr = true;
                        $scope.classdrdr1 = true;
                        $scope.cempgrpdr = false;
                        $scope.cempgrpdr1 = false;
                        $scope.depgeust = false;
                        $scope.clgclassdrdr = false;
                        $scope.clgclassdrdrnon = false;
                        $scope.booktypedr = true;
                        $scope.nonbookstff = false;
                        $scope.nondepgeust = false;
                        $scope.nonclassdrdr = false;
                    }
                    else if ($scope.schclgflag == 'C' || $scope.schclgflag =='U') {
                        $scope.classdrdr = false;
                        $scope.classdrdr1 = false;
                        $scope.cempgrpdr = false;
                        $scope.cempgrpdr1 = false;
                        $scope.depgeust = false;
                        $scope.clgclassdrdr = true;
                        $scope.booktypedr = true;
                        $scope.nonbookstff = false;
                        $scope.nondepgeust = false;
                        $scope.clgclassdrdrnon = false;
                        $scope.nonclassdrdr = false;
                    }
                  


                }
                else if ($scope.book_Flag == "BP" && $scope.issuertype1 == 'STAFF') {
                    $scope.classdrdr = false;
                    $scope.classdrdr1 = false;
                    $scope.cempgrpdr = true;
                    $scope.cempgrpdr1 = true;
                    $scope.depgeust = false;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = true;
                    $scope.nonbookstff = false;
                    $scope.nondepgeust = false;
                    $scope.clgclassdrdrnon = false;
                    $scope.nonclassdrdr = false;
                }
                else if ($scope.book_Flag == "BP" && $scope.issuertype1 == 'DEPARTMENT') {
                    $scope.classdrdr = false;
                    $scope.classdrdr1 = false;
                    $scope.cempgrpdr = false;
                    $scope.cempgrpdr1 = false;
                    $scope.depgeust = true;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = true;
                    $scope.nonbookstff = false;
                    $scope.nondepgeust = false;
                    $scope.clgclassdrdrnon = false;
                    $scope.nonclassdrdr = false;
                }
                else if ($scope.book_Flag == "BP" && $scope.issuertype1 == 'GUEST') {
                    $scope.classdrdr = false;
                    $scope.cempgrpdr = false;
                    $scope.cempgrpdr1 = false;
                    $scope.depgeust = true;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = true;
                    $scope.nonbookstff = false;
                    $scope.nondepgeust = false;
                    $scope.clgclassdrdrnon = false;
                    $scope.nonclassdrdr = false;
                }

                if ($scope.book_Flag == "NBP" && $scope.issuertype1 == 'STUDENT') {

                    if ($scope.schclgflag == 'S') {
                        $scope.classdrdr = false;
                        $scope.classdrdr1 = true;
                        $scope.cempgrpdr = false;
                        $scope.cempgrpdr1 = false;
                        $scope.depgeust = false;
                        $scope.clgclassdrdr = false;
                        $scope.booktypedr = false;
                        $scope.nonclassdrdr = true;
                        $scope.nonbookstff = false;
                        $scope.nondepgeust = false;
                        $scope.clgclassdrdrnon = false;
                    }
                    else if ($scope.schclgflag == 'C') {
                        $scope.classdrdr = false;
                        $scope.classdrdr1 = false;
                        $scope.cempgrpdr = false;
                        $scope.cempgrpdr1 = false;
                        $scope.depgeust = false;
                        $scope.clgclassdrdr = false;
                        $scope.booktypedr = false;
                        $scope.nonclassdrdr = false;
                        $scope.nonbookstff = false;
                        $scope.nondepgeust = false;
                        $scope.clgclassdrdrnon = true;

                    }



                }
                else if ($scope.book_Flag == "NBP" && $scope.issuertype1 == 'STAFF') {
                    $scope.classdrdr = false;
                    $scope.classdrdr1 = false;
                    $scope.cempgrpdr = false;
                    $scope.cempgrpdr1 = true;
                    $scope.depgeust = false;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = false;
                    $scope.nonclassdrdr = false;
                    $scope.nonbookstff = true;
                    $scope.nondepgeust = false;
                    $scope.clgclassdrdrnon = false;
                }
                else if ($scope.book_Flag == "NBP" && $scope.issuertype1 == 'DEPARTMENT') {
                    $scope.classdrdr = false;
                    $scope.classdrdr1 = false;
                    $scope.cempgrpdr = false;
                    $scope.cempgrpdr1 = false;
                    $scope.depgeust = false;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = false;
                    $scope.nonclassdrdr = false;
                    $scope.nonbookstff = false;
                    $scope.nondepgeust = true;
                    $scope.clgclassdrdrnon = false;
                }
                else if ($scope.book_Flag == "NBP" && $scope.issuertype1 == 'GUEST') {
                    $scope.classdrdr = false;
                    $scope.classdrdr1 = false;
                    $scope.cempgrpdr = false;
                    $scope.cempgrpdr1 = false;
                    $scope.depgeust = false;
                    $scope.clgclassdrdr = false;
                    $scope.booktypedr = false;
                    $scope.nonclassdrdr = false;
                    $scope.nonbookstff = false;
                    $scope.nondepgeust = true;
                    $scope.clgclassdrdrnon = false;
                }
                $scope.categlst = promise.categorylist;
                $scope.alldata = promise.alldata;
                $scope.fillemp = promise.fillemp;
                $scope.classcount = promise.fillclass;
            })
        }
          //=====================End-----Loaddata----//


        $scope.onselectcategory = function () {
            debugger;


            //angular.forEach($scope.categlst, function (y) {
            //    if (y.lmC_Id == $scope.LMC_Id) {
            //        $scope.acdyr = y.lmC_CategoryName;
            //    }
            //})

            var data = {
                "LMC_Id": $scope.LMC_Id,
                "Catgname": $scope.LMB_BookType,
                "LMC_CategoryName": $scope.book_Flag,
            }
            apiService.create("CirculationParameter/gettype", data).
                then(function (promise) {
                    $scope.issuetype = promise.issuetype;
                })
        }


        $scope.onselecttype = function () {
            angular.forEach($scope.issuetype, function (y) {
                if (y.lbcpA_Id == $scope.LBCPA_Id) {
                    $scope.lbcpA_Flg = y.lbcpA_Flg;
                }
                if ($scope.lbcpA_Flg == 'STUDENT') {
                    $scope.std = true;
                    $scope.stf = false;
                }
                else if ($scope.lbcpA_Flg == 'STAFF') {
                    $scope.stf = true;
                    $scope.std = false;
                }
                else{
                    $scope.stf = false;
                    $scope.std = false;
                }
            })
        }

        $scope.cleardata = function () {
            $scope.Max_Issue_Items = '';
            $scope.Max_Issue_Days = '';
            $scope.Max_No_Renewals = '';
            $scope.ASMCL_Id = '';
            $scope.LBCPA_Id = '';
            $scope.LMB_BookType = '';
        }

         //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.book_Flag=='BP') {
                    if ($scope.issuertype1 == 'STUDENT') {
                        $scope.Circ_Flag = "std"
                        var data = {
                            
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "ASMCL_Id": $scope.ASMCL_Id,
                            "LBCPA_Id": $scope.LBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "Catgname": $scope.LMB_BookType,
                        }
                    }
                    else if ($scope.issuertype1 == 'STAFF') {
                        $scope.Circ_Flag = "stf"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                          
                            "HRMGT_Id": $scope.HRMGT_Id,
                            "LBCPA_Id": $scope.LBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "Catgname": $scope.LMB_BookType,
                        }
                    }
                    else if ($scope.issuertype1 == 'GUEST') {
                        $scope.Circ_Flag = "gst"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "LBCPA_Id": $scope.LBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "Catgname": $scope.LMB_BookType,
                        }
                    }
                    else if ($scope.issuertype1 == 'DEPARTMENT') {
                        $scope.Circ_Flag = "dpt"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "LBCPA_Id": $scope.LBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "Catgname": $scope.LMB_BookType,
                        }
                    }
                }
                else if ($scope.book_Flag == 'NBP') {
                    if ($scope.issuertype1 == 'STUDENT') {
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "ASMCL_Id": $scope.ASMCL_Id,
                            "LNBCPA_Id": $scope.LNBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                             "LMC_Id": $scope.LMC_Id,
                        }
                    }
                    else if ($scope.issuertype1 == 'STAFF') {
                        $scope.Circ_Flag = "stf"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,

                            "HRMGT_Id": $scope.HRMGT_Id,
                            "LNBCPA_Id": $scope.LNBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "LMC_Id": $scope.LMC_Id,
                            
                        }
                    }
                    else if ($scope.issuertype1 == 'GUEST') {
                        $scope.Circ_Flag = "gst"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "LNBCPA_Id": $scope.LNBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "LMC_Id": $scope.LMC_Id,
                            
                        }
                    }
                    else if ($scope.issuertype1 == 'DEPARTMENT') {
                        $scope.Circ_Flag = "dpt"
                        var data = {
                            "Max_Issue_Items": $scope.Max_Issue_Items,
                            "Max_Issue_Days": $scope.Max_Issue_Days,
                            "Max_No_Renewals": $scope.Max_No_Renewals,
                            "LNBCPA_Id": $scope.LNBCPA_Id,
                            "BOOKFLAG": $scope.book_Flag,
                            "issuertype1": $scope.issuertype1,
                            "LMC_Id": $scope.LMC_Id,

                           
                        }
                    }
                }
                apiService.create("CirculationParameter/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.Circ_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.Circ_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            //$scope.Loaddata();
                            //$scope.cleardata();
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };
         //=====================End---saverecord....


         //=====================Editrecord....
        $scope.EditData = function (user) {
            debugger;

            if ($scope.book_Flag=='BP') {
                if (user.LBCPA_Flg == 'STUDENT') {

                    if ($scope.schclgflag=='S') {
                        $scope.LBCPA_Id = user.LBCPA_Id;
                        $scope.ASMCL_Id = user.ASMCL_Id;
                        $scope.Max_Issue_Items = user.LBCPAS_NoOfItems;
                        $scope.Max_Issue_Days = user.LBCPAS_IssueDays;
                        $scope.Max_No_Renewals = user.LBCPAS_NoOfRenewals;
                        $scope.LMB_BookType = user.LBCPA_IssueRefFlg;
                    }
                    else if ($scope.schclgflag == 'C') {
                        $scope.LBCPA_Id = user.LBCPA_Id;
                      //  $scope.ASMCL_Id = user.ASMCL_Id;
                        $scope.Max_Issue_Items = user.LBCPASC_NoOfItems;
                        $scope.Max_Issue_Days = user.LBCPASC_IssueDays;
                        $scope.Max_No_Renewals = user.LBCPASC_NoOfRenewals;
                        $scope.LMB_BookType = user.LBCPA_IssueRefFlg;
                    }
                   
                }
                else if (user.LBCPA_Flg == 'STAFF') {
                    $scope.LBCPA_Id = user.LBCPA_Id;
                    $scope.HRMGT_Id = user.HRMGT_Id;
                    $scope.Max_Issue_Items = user.LBCPAST_NoOfItems;
                    $scope.Max_Issue_Days = user.LBCPAST_IssueDays;
                    $scope.Max_No_Renewals = user.LBCPAST_NoOfRenewals;
                    $scope.LMB_BookType = user.LBCPA_IssueRefFlg;
                }
                else if (user.LBCPA_Flg == 'GUEST' || user.LBCPA_Flg == 'DEPARTMENT') {
                    $scope.LBCPA_Id = user.LBCPA_Id;
                    $scope.Max_Issue_Items = user.LBCPAO_NoOfItems;
                    $scope.Max_Issue_Days = user.LBCPAO_IssueDays;
                    $scope.Max_No_Renewals = user.LBCPAO_NoOfRenewals;
                    $scope.LMB_BookType = user.LBCPA_IssueRefFlg;
                }
            }
            else if ($scope.book_Flag == 'NBP') {

                if (user.LNBCPA_Flg == 'STUDENT') {

                    if ($scope.schclgflag == 'S') {
                        $scope.LNBCPA_Id = user.LNBCPA_Id;
                        $scope.ASMCL_Id = user.ASMCL_Id;
                        $scope.LMC_Id = user.LMC_Id;
                        $scope.Max_Issue_Items = user.LNBCPAS_NoOfItems;
                        $scope.Max_Issue_Days = user.LNBCPAS_IssueDays;
                        $scope.Max_No_Renewals = user.LNBCPAS_NoOfRenewals;
                    }
                    else {
                        $scope.LNBCPA_Id = user.LNBCPA_Id;
                        $scope.LMC_Id = user.LMC_Id;
                        $scope.Max_Issue_Items = user.LNBCPASC_NoOfItems;
                        $scope.Max_Issue_Days = user.LNBCPASC_IssueDays;
                        $scope.Max_No_Renewals = user.LNBCPASC_NoOfRenewals;
                    }
                }
                else if (user.LNBCPA_Flg == 'STAFF') {
                    $scope.LNBCPA_Id = user.LNBCPA_Id;
                    $scope.HRMGT_Id = user.HRMGT_Id;
                    $scope.LMC_Id = user.LMC_Id;
                    $scope.Max_Issue_Items = user.LNBCPAST_NoOfItems;
                    $scope.Max_Issue_Days = user.LNBCPAST_IssueDays;
                    $scope.Max_No_Renewals = user.LNBCPAST_NoOfRenewals;
                  
                }
                else if (user.LNBCPA_Flg == 'GUEST' || user.LNBCPA_Flg == 'DEPARTMENT') {
                    $scope.LNBCPA_Id = user.LNBCPA_Id;
                    $scope.LMC_Id = user.LMC_Id;
                    $scope.Max_Issue_Items = user.LNBCPAO_NoOfItems;
                    $scope.Max_Issue_Days = user.LNBCPAO_IssueDays;
                    $scope.Max_No_Renewals = user.LNBCPAO_NoOfRenewals;
                   
                }
            }
            
            
            debugger;
            //apiService.create("CirculationParameter/deactiveY", user).
            //    then(function (promise) {
                  
            //    })
        }
         //====================End---editrecord....
       
        
         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            var Parmeter_Id = 0;
            if ($scope.classdrdr == true) {
                Parmeter_Id = user.LBCPAS_Id
            }
            ////cempgrpdr
            //if ($scope.cempgrpdr == true) {
            //    Parmeter_Id = user.LBCPASC_Id
            //}
            //LBCPASC_Id
            var dystring = "";
            if ($scope.book_Flag=='BP') {
                var data = {
                    "LBCPA_Id": user.LBCPA_Id,
                    "BOOKFLAG": $scope.book_Flag,
                    "issuertype1": $scope.issuertype1,
                    "MI_SchoolCollegeFlag": $scope.schclgflag,
                    "Parmeter_Id": Parmeter_Id
                }


                if (user.LBCPA_ActiveFlg == true) {
                    dystring = "Deactivate";
                }
                else if (user.LBCPA_ActiveFlg == false) {
                    dystring = "Activate";
                }
            }
            else if ($scope.book_Flag == 'NBP') {
                var data = {
                    "LNBCPA_Id": user.LNBCPA_Id,
                    "BOOKFLAG": $scope.book_Flag,
                    "issuertype1": $scope.issuertype1,
                    "MI_SchoolCollegeFlag": $scope.schclgflag,
                }
                if (user.LNBCPA_ActiveFlg == true) {
                    dystring = "Deactivate";
                }
                else if (user.LNBCPA_ActiveFlg == false) {
                    dystring = "Activate";
                }
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
                        apiService.create("CirculationParameter/deactiveY", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                               // $state.reload();
                                $scope.Loaddata();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        }
         //================End----Activation/Deactivation--Record.........



 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

          //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

