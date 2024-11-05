
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeWizardController', FeeWizardController)

    FeeWizardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$compile']
    function FeeWizardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $compile) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.AMAY_Id_PS1dis = true;
        $scope.sortKey = "fmG_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.userPrivileges = "";
        $scope.sortKey1 = "fyG_Id";
        $scope.reverse1 = true;
        $scope.enablefeegroup = false;
        $scope.enablefeeheadgroup = false;
        $scope.enableclasscategory = false;
        $scope.enableamountentry = false;

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmG_ActiceFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmG_ActiceFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.fmG_Remarks).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.searchValue1 = "";
        $scope.searchValue2 = "";
        $scope.filterValue1 = function (obj) {
            if (obj.fyG_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fyG_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.grpname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase(obj.yearname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue1)) >= 0;
        }
        $scope.isOptionsRequired = function () {
            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        $scope.savedisable = true;
        $scope.editdisable = true;
        $scope.deletedisable = true;
        $scope.userPrivileges = [];
        var pageid = $stateParams.pageId;
       // var pageid = 2;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;
                    $scope.savedisable = true;
                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;

                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;
                    $scope.editdisable = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;

                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                    $scope.deletedisable = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;

                }


            }
        }



        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.itemsPerPage2 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
        
            apiService.getURI("FeeWizard/getalldetails", pageid).
                then(function (promise) {
                
                     $scope.AMAY_Id_PS = academicyrlst[0].asmaY_Id;

                    $scope.yearList = promise.academicdrp;

                    $scope.yearlyfeegroupdata = promise.yearlygroup;

                    $scope.AMAY_Id_FGH = academicyrlst[0].asmaY_Id;

                    $scope.yearListFGH = promise.academicdrp;

                    $scope.yearlyfeegroupheaddata = promise.yearlygrouphead;


                    $scope.AMAY_Id_YCC = academicyrlst[0].asmaY_Id;

                    $scope.yearListYCC = promise.academicdrp;

                    $scope.classcategorydata = promise.classcategorydata;

                    $scope.AMAY_Id_FMA = academicyrlst[0].asmaY_Id;

                    $scope.yearListFMA = promise.academicdrp;
                    $scope.amountentrydata = promise.amountentrydata;
                    $scope.AMAY_Id_FMAG = academicyrlst[0].asmaY_Id;

                    $scope.yearListFMAG = promise.academicdrp;


                    $scope.autoreceiptdata = promise.autoreceiptdata;


                })
        }

        //Fee yearly group
        $scope.saveYearlyGroupdata = function () {

            $scope.finaldataPS = [];
            angular.forEach($scope.Feeyearlygroupresult, function (student) {
                if (student.Selected1_PS == true) {
                    $scope.finaldataPS.push(student);
                }
            });
            if ($scope.finaldataPS.length > 0) {

                var plg = $scope.finaldataPS;
                var data = {
                    "ASMAY_Idnew": $scope.AMAY_Id_PS1,
                    
                
                    resultData: plg
                }
                apiService.create("FeeWizard/savedetailY", data).then(function (promise) {
                    if (promise.returnduplicatestatus == 'Duplicate') {
                                swal("Record Already Exist");
                                $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        
                            }

                            else if (promise.returnduplicatestatus == "Save") {
                                $scope.students = promise.groupData;
                                swal('Record Saved Successfully');
                                $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                            }

                            else if (promise.returnduplicatestatus == "NotSave") {

                                swal('Record Not Saved');
                                $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                            }
                            else if (promise.returnduplicatestatus == "Update") {
                                $scope.students = promise.groupData;
                                swal('Record Updated Successfully');
                                $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                            }
                            else if (promise.returnduplicatestatus == "NotUpdate") {

                                swal('Record Not Updated');
                                $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                            }
                    else {
                        swal('Request Failed');
                        $scope.enablefeegroup = false;
                    }
                    $state.reload();
                });
            }
            else {
                swal('Kindly add atleast one record to second grid');
                return;
            }
           

        };
      
        $scope.Feeyearlygroupresult = [];
        $scope.GetFirstTablePS = function () {
          
            if ($scope.selectedAll_PS == true) {
                angular.forEach($scope.Feeyearlygroup, function (student) {
                    $scope.Feeyearlygroupresult.push(student);
                });
            } else {
                angular.forEach($scope.Feeyearlygroup, function (student) {
                    if (student.Selected_PS == true) {
                        $scope.Feeyearlygroupresult.push(student);
                    }
                });
            }
            $scope.Feeyearlygroup = $scope.Feeyearlygroup.filter(function (student) {
                return !student.Selected_PS
            })

            $scope.checkAll1_PS();
            $scope.checkAll_PS();
        };

        $scope.RemoveSecondTablePS = function () {
            if ($scope.selectedAll1_PS == true) {
                angular.forEach($scope.Feeyearlygroupresult, function (student) {
                    $scope.Feeyearlygroup.push(student);
                });
            } else {
                angular.forEach($scope.Feeyearlygroupresult, function (student) {
                    if (student.Selected1_PS == true) {
                        $scope.Feeyearlygroup.push(student);
                    }
                });
            }
            $scope.Feeyearlygroupresult = $scope.Feeyearlygroupresult.filter(function (student) {
                return !student.Selected1_PS;
            })
            $scope.checkAll_PS();
            $scope.checkAll1_PS();
        };

        $scope.OnChangeAcademicYearps = function () {
            $scope.NoOfYears = "";
            $scope.Feeyearlygroup = [];
            $scope.Feeyearlygroupresult = [];
            

          
        };


        $scope.cance1 = function (arrlistchk) {
           
            $scope.submitted1 = false;
           


        }


        $scope.OnChangeNoOfYears = function () {

            var data = {

               
                "ASMAY_Id": $scope.AMAY_Id_PS,
                "ASMAY_Order": $scope.NoOfYears,
                
            }
            apiService.create("FeeWizard/changacademicyear", data).
                then(function (promise) {
                    $scope.AMAY_Id_PS1 = promise.academicyearnew[0].asmaY_Id;
                    $scope.yearList1ps = promise.academicyearnew;

                    $scope.Feeyearlygroup = promise.groupYearData;
                    $scope.enablefeegroup = true;

                   
                })


        }
        $scope.test_PS = function (data) {
            $scope.selectedAll_PS = $scope.Feeyearlygroup.every(function (itm) {
                return itm.Selected_PS;
            })
        };

        $scope.test1_PS = function (data) {
            $scope.selectedAll1_PS = $scope.Feeyearlygroupresult.every(function (itm) { return itm.Selected1_PS; })
        };

        $scope.chckedIndexs_PS = [];
        $scope.chckedIndexs1_PS = [];
        $scope.checkAll_PS = function () {
            if ($scope.Feeyearlygroup.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PS;
                angular.forEach($scope.Feeyearlygroup, function (itm) {
                    itm.Selected_PS = toggleStatus_PS;
                    if ($scope.chckedIndexs_PS.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PS.splice($scope.chckedIndexs_PS.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PS = false;
            }
        };

        $scope.checkAll1_PS = function () {

            if ($scope.Feeyearlygroupresult.length > 0) {
                $scope.selectedAll1_PS = true;
                var toggleStatus_PS = $scope.selectedAll1_PS;
                angular.forEach($scope.Feeyearlygroupresult, function (itm) {
                    itm.Selected1_PS = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PS.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PS.splice($scope.chckedIndexs1_PS.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PS = false;
            }
        };

        $scope.deactiveY = function (newary, SweetAlert) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var confirmmgs = "";
            if (newary.fyG_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeWizard/deactivateY", newary).
                            then(function (promise) {
                                $scope.yearlyfeegroupdata = promise.yearlygroup;
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " Successfully");
                                }
                                else {
                                    swal("Record is already been used !!!");
                                }
                                $scope.loaddata();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.Clearid_ps = function () {
           
            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PS = "";
            $scope.NoOfYears = "";
         
            $scope.selectedAll_PS = false;
            $scope.selectedAll1_PS = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PS = [];
            $scope.Feeyearlygroup = [];
 
            $scope.Feeyearlygroupresult = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];
      
            $scope.submittedPS = false;
            $scope.enablefeegroup = false;
            $scope.AMAY_Id_PS = "";
            $scope.NoOfYears = "";
            $scope.AMAY_Id_PS1 = "";


        };

         //Fee yearly group

        $scope.OnChangeAcademicYearps = function () {
            $scope.NoOfYearsFGH = "";
            $scope.Feeyearlygrouphead = [];
            $scope.Feeyearlygroupheadresult = [];
          


        };

        $scope.test_PSFGH = function (data) {
            $scope.selectedAll_PSFGH = $scope.Feeyearlygrouphead.every(function (itm) {
                return itm.Selected_PSFGH;
            })
        };

        $scope.test1_PSFGH = function (data) {
            $scope.selectedAll1_PSFGH = $scope.FeeyearlygroupheadRESULT.every(function (itm) { return itm.Selected1_PSFGH; })
        };

        $scope.chckedIndexs_PSFGH = [];
        $scope.chckedIndexs1_PSFGH = [];
        $scope.checkAll_PSFGH = function () {
            if ($scope.Feeyearlygrouphead.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PSFGH;
                angular.forEach($scope.Feeyearlygrouphead, function (itm) {
                    itm.Selected_PSFGH = toggleStatus_PS;
                    if ($scope.chckedIndexs_PSFGH.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PSFGH.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PSFGH.splice($scope.chckedIndexs_PSFGH.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PSFGH = false;
            }
        };

        $scope.checkAll1_PSFGH = function () {

            if ($scope.Feeyearlygroupheadresult.length > 0) {
                $scope.selectedAll1_PSFGH = true;
                var toggleStatus_PS = $scope.selectedAll1_PSFGH;
                angular.forEach($scope.Feeyearlygroupheadresult, function (itm) {
                    itm.Selected1_PSFGH = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PSFGH.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PSFGH.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PSFGH.splice($scope.chckedIndexs1_PSFGH.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PSFGH = false;
            }
        };


        $scope.Feeyearlygroupheadresult = [];
        $scope.GetFirstTablePSFGH = function () {

            if ($scope.selectedAll_PSFGH == true) {
                angular.forEach($scope.Feeyearlygrouphead, function (student) {
                    $scope.Feeyearlygroupheadresult.push(student);
                });
            } else {
                angular.forEach($scope.Feeyearlygrouphead, function (student) {
                    if (student.Selected_PSFGH == true) {
                        $scope.Feeyearlygroupheadresult.push(student);
                    }
                });
            }
            $scope.Feeyearlygrouphead = $scope.Feeyearlygrouphead.filter(function (student) {
                return !student.Selected_PSFGH
            })

            $scope.checkAll1_PSFGH();
            $scope.checkAll_PSFGH();
        };

        $scope.RemoveSecondTablePSFGH = function () {
            if ($scope.selectedAll1_PS == true) {
                angular.forEach($scope.Feeyearlygroupheadresult, function (student) {
                    $scope.Feeyearlygrouphead.push(student);
                });
            } else {
                angular.forEach($scope.Feeyearlygroupheadresult, function (student) {
                    if (student.Selected1_PSFGH == true) {
                        $scope.Feeyearlygrouphead.push(student);
                    }
                });
            }
            $scope.Feeyearlygroupheadresult = $scope.Feeyearlygroupheadresult.filter(function (student) {
                return !student.Selected1_PSFGH;
            })
            $scope.checkAll_PSFGH();
            $scope.checkAll1_PSFGH();
        };

        $scope.OnChangeNoOfYearshead = function () {

            var data = {


                "ASMAY_Id": $scope.AMAY_Id_FGH,
                "ASMAY_Order": $scope.NoOfYearsFGH,

            }
            apiService.create("FeeWizard/changacademicyear", data).
                then(function (promise) {


                    $scope.enablefeeheadgroup = true;
                    $scope.AMAY_Id_PSFGH1 = promise.academicyearnew[0].asmaY_Id;
                    $scope.yearListFGH1 = promise.academicyearnew;
                    $scope.Feeyearlygrouphead = promise.yearlygroupheaddata;

                })


        }


        $scope.saveYearlyGroupHeaddata = function () {

            $scope.finaldataPS = [];
            angular.forEach($scope.Feeyearlygroupheadresult, function (student) {
                if (student.Selected1_PSFGH == true) {
                    $scope.finaldataPS.push(student);
                }
            });
            if ($scope.finaldataPS.length > 0) {

                var plg = $scope.finaldataPS;
                var data = {
                    "ASMAY_Idnew": $scope.AMAY_Id_PSFGH1,


                    resultData: plg
                }
                apiService.create("FeeWizard/savedetailFGH", data).then(function (promise) {
                    if (promise.returnduplicatestatus == 'FineHead') {
                        swal("Add the Fine Head");
                        $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();

                    }

                    else if (promise.returnduplicatestatus == "Save") {
                        $scope.students = promise.groupData;
                        swal('Record Saved Successfully');
                        $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                    }

                    else if (promise.returnduplicatestatus == "NotSave") {

                        swal('Record Not Saved');
                        $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                    }
                    else if (promise.returnduplicatestatus == "Update") {
                        $scope.students = promise.groupData;
                        swal('Record Updated Successfully');
                        $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                    }
                    else if (promise.returnduplicatestatus == "NotUpdate") {

                        swal('Record Not Updated');
                        $scope.cance1($scope.arrlistchk);
                        $scope.loaddata();
                        $scope.enablefeegroup = false;
                    }
                    else {
                        swal('Request Failed');
                        $scope.enablefeegroup = false;
                    }
                    $state.reload();
                });
            }
            else {
                swal('Kindly add atleast one record to second grid');
                return;
            }


        };

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("FeeWizard/Deletedetails", orgid).
                            then(function (promise) {

                                $scope.amountentrydata = promise.amountentrydata;
                                

                                if (promise.returnval == true) {

                                    $scope.masterse = promise.masterSectionData;

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                                $scope.formload();
                            })
                        $scope.formload();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.formload();
                    }
                });


            //})
        }

        $scope.Clearid_psfgh = function () {

            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PSFGH = "";
            $scope.NoOfYearsFGH = "";
           
            $scope.selectedAll_PSFGH = false;
            $scope.selectedAll1_PSFGH = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PSFGH = [];
            $scope.Feeyearlygrouphead = [];

            $scope.Feeyearlygroupheadresult = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];

            $scope.enablefeeheadgroup = false;
            $scope.AMAY_Id_FGH = "";
            $scope.NoOfYearsFGH = "";
            $scope.AMAY_Id_PSFGH1 = "";


        };
            //Fee yearly group

        //class category data
        $scope.test_PSYCC = function (data) {
            $scope.selectedAll_PSYCC = $scope.classcategory.every(function (itm) {
                return itm.Selected_PSYCC;
            })
        };

        $scope.test1_PSYCC = function (data) {
            $scope.selectedAll1_PSYCC = $scope.classcategoryresult.every(function (itm) { return itm.Selected1_PSYCC; })
        };

        $scope.chckedIndexs_PSYCC = [];
        $scope.chckedIndexs1_PSYCC = [];
        $scope.checkAll_PSYCC = function () {
            if ($scope.classcategory.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PSYCC;
                angular.forEach($scope.classcategory, function (itm) {
                    itm.Selected_PSYCC = toggleStatus_PS;
                    if ($scope.chckedIndexs_PSYCC.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PSYCC.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PSYCC.splice($scope.chckedIndexs_PSYCC.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PSYCC = false;
            }
        };

        $scope.checkAll1_PSYCC = function () {

            if ($scope.classcategoryresult.length > 0) {
                $scope.selectedAll1_PSYCC = true;
                var toggleStatus_PS = $scope.selectedAll1_PSYCC;
                angular.forEach($scope.classcategoryresult, function (itm) {
                    itm.Selected1_PSYCC = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PSYCC.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PSYCC.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PSYCC.splice($scope.chckedIndexs1_PSYCC.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PSYCC = false;
            }
        };


        $scope.classcategoryresult = [];
        $scope.GetFirstTablePSYCC = function () {

            if ($scope.selectedAll_PSYCC == true) {
                angular.forEach($scope.classcategory, function (student) {
                    $scope.classcategoryresult.push(student);
                });
            } else {
                angular.forEach($scope.classcategory, function (student) {
                    if (student.Selected_PSYCC == true) {
                        $scope.classcategoryresult.push(student);
                    }
                });
            }
            $scope.classcategory = $scope.classcategory.filter(function (student) {
                return !student.Selected_PSYCC
            })

            $scope.checkAll1_PSYCC();
            $scope.checkAll_PSYCC();
        };

        $scope.RemoveSecondTablePSYCC = function () {
            if ($scope.selectedAll1_PS == true) {
                angular.forEach($scope.classcategoryresult, function (student) {
                    $scope.classcategory.push(student);
                });
            } else {
                angular.forEach($scope.classcategoryresult, function (student) {
                    if (student.Selected1_PSYCC == true) {
                        $scope.classcategory.push(student);
                    }
                });
            }
            $scope.classcategoryresult = $scope.classcategoryresult.filter(function (student) {
                return !student.Selected1_PSYCC;
            })
            $scope.checkAll_PSYCC();
            $scope.checkAll1_PSYCC();
        };

        $scope.OnChangeAcademicYearYCC = function () {

            var data = {


                "ASMAY_Id": $scope.AMAY_Id_YCC,
                "ASMAY_Order": $scope.NoOfYearsYCC,

            }
            apiService.create("FeeWizard/changacademicyear", data).
                then(function (promise) {


                    $scope.enableclasscategory =  true;
                    $scope.AMAY_Id_PSYCC1 = promise.academicyearnew[0].asmaY_Id;
                    $scope.yearListYCC1 = promise.academicyearnew;
                    $scope.classcategory = promise.classcategory;

                })


        }


        $scope.saveclasscategorydata = function () {

            $scope.finaldataPS = [];
            angular.forEach($scope.classcategoryresult, function (student) {
                if (student.Selected1_PSYCC == true) {
                    $scope.finaldataPS.push(student);
                }
            });
            if ($scope.finaldataPS.length > 0) {

                var plg = $scope.finaldataPS;
                var data = {
                    "ASMAY_Idnew": $scope.AMAY_Id_PSYCC1,


                    resultData: plg
                }
                apiService.create("FeeWizard/savedetailYCC", data).then(function (promise) {
                    if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Updated Successfully', 'success');
                        }
                        else {
                            swal('Record Saved Successfully', 'success');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                swal('Kindly add atleast one record to second grid');
                return;
            }


        };

        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fycC_Id;
            var pageid = $scope.editEmployeeY;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("FeeWizard/deletepagesY", pageid).
                            then(function (promise) {
                                $scope.loaddata();
                                $scope.students = promise.claSSCategoryArray;
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }

        $scope.Clearid_psycc = function () {

            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PSYCC = "";
            $scope.NoOfYearsYCC = "";

            $scope.selectedAll_PSYCC = false;
            $scope.selectedAll1_PSYCC = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PSYCC = [];
            $scope.Feeyearlygrouphead = [];

            $scope.Feeyearlygroupheadresult = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];

            $scope.enableclasscategory = false;
            $scope.AMAY_Id_YCC = "";
            $scope.NoOfYearsYCC = "";
            $scope.AMAY_Id_PSYCC1 = "";


        };
        //class category data


        //Amount Entry


        $scope.test_PSFMA = function (data) {
            $scope.selectedAll_PSFMA = $scope.amountentry.every(function (itm) {
                return itm.Selected_PSFMA;
            })
        };

        $scope.test1_PSFMA = function (data) {
            $scope.selectedAll1_PSFMA = $scope.amountentryresult.every(function (itm) { return itm.Selected1_PSFMA; })
        };

        $scope.chckedIndexs_PSFMA = [];
        $scope.chckedIndexs1_PSFMA = [];
        $scope.checkAll_PSFMA = function () {
            if ($scope.amountentry.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PSFMA;
                angular.forEach($scope.amountentry, function (itm) {
                    itm.Selected_PSFMA = toggleStatus_PS;
                    if ($scope.chckedIndexs_PSFMA.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PSFMA.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PSFMA.splice($scope.chckedIndexs_PSFMA.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PSYCC = false;
            }
        };

        $scope.checkAll1_PSFMA = function () {

            if ($scope.amountentryresult.length > 0) {
                $scope.selectedAll1_PSFMA = true;
                var toggleStatus_PS = $scope.selectedAll1_PSFMA;
                angular.forEach($scope.amountentryresult, function (itm) {
                    itm.Selected1_PSFMA = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PSFMA.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PSFMA.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PSFMA.splice($scope.chckedIndexs1_PSFMA.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PSFMA = false;
            }
        };


        $scope.amountentryresult = [];
        $scope.GetFirstTablePSFMA = function () {

            if ($scope.selectedAll_PSFMA == true) {
                angular.forEach($scope.amountentry, function (student) {
                    $scope.amountentryresult.push(student);
                });
            } else {
                angular.forEach($scope.amountentry, function (student) {
                    if (student.Selected_PSFMA == true) {
                        $scope.amountentryresult.push(student);
                    }
                });
            }
            $scope.amountentry = $scope.amountentry.filter(function (student) {
                return !student.Selected_PSFMA
            })

            $scope.checkAll1_PSFMA();
            $scope.checkAll_PSFMA();
        };

        $scope.RemoveSecondTablePSFMA = function () {
            if ($scope.selectedAll1_PS == true) {
                angular.forEach($scope.amountentryresult, function (student) {
                    $scope.amountentry.push(student);
                });
            } else {
                angular.forEach($scope.classcategoryresult, function (student) {
                    if (student.Selected1_PSFMA == true) {
                        $scope.amountentry.push(student);
                    }
                });
            }
            $scope.amountentryresult = $scope.amountentryresult.filter(function (student) {
                return !student.Selected1_PSFMA;
            })
            $scope.checkAll_PSFMA();
            $scope.checkAll1_PSFMA();
        };

        $scope.OnChangeAcademicYearFMA = function () {

            var data = {


                "ASMAY_Id": $scope.AMAY_Id_FMA,
                "ASMAY_Order": $scope.NoOfYearsFMA,

            }
            apiService.create("FeeWizard/changacademicyear", data).
                then(function (promise) {


                    $scope.enableamountentry = true;
                    $scope.AMAY_Id_PSFMA1 = promise.academicyearnew[0].asmaY_Id;
                    $scope.yearListFMA1 = promise.academicyearnew;
                    $scope.amountentry = promise.amountentry;

                })


        }


        $scope.saveamountentrydata = function () {

            $scope.finaldataPS = [];
            angular.forEach($scope.amountentryresult, function (student) {
                if (student.Selected1_PSFMA == true) {
                    $scope.finaldataPS.push(student);
                }
            });
            if ($scope.finaldataPS.length > 0) {

                var plg = $scope.finaldataPS;
                var data = {
                    "ASMAY_Idnew": $scope.AMAY_Id_PSFMA1,


                    resultData: plg
                }
                apiService.create("FeeWizard/savedetailFMA", data).then(function (promise) {
                    if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Updated Successfully', 'success');
                        }
                        else {
                            swal('Record Saved Successfully', 'success');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                swal('Kindly add atleast one record to second grid');
                return;
            }


        };

        $scope.DeletRecordFMA = function (amountid, selectiontype) {

            var data = {
                "FMA_Id": amountid,
                "selectiontype": selectiontype
            }
            var amtid = amountid;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeWizard/Deletedetails/", data).
                            then(function (promise) {

                                $scope.amountentrydata = promise.amountentrydata;
                                if (promise.returnduplicatestatus == "true") {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.returnduplicatestatus == "false") {
                                    swal('Contact Administrator');
                                }
                                else if (promise.returnduplicatestatus == "RecordExists") {
                                    swal('Data has already been used in Transactions!! So record cannot be deleted');
                                }
                                $scope.formload();

                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });

        }

        $scope.Clearid_psFMA = function () {

            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PSFMA = "";
            $scope.NoOfYearsFMA = "";

            $scope.selectedAll_PSFMA = false;
            $scope.selectedAll1_PSFMA = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PSFMA = [];
            $scope.amountentry = [];

            $scope.amountentryresult = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];

            $scope.enableamountentry = false;
            $scope.AMAY_Id_FMA = "";
            $scope.NoOfYearsFMA = "";
            $scope.AMAY_Id_PSFMA1 = "";

        };

        //Amount Entry

        //Auto Receipt


        $scope.test_PSFMAG = function (data) {
            $scope.selectedAll_PSFMAG = $scope.autoreceipt.every(function (itm) {
                return itm.Selected_PSFMAG;
            })
        };

        $scope.test1_PSFMAG = function (data) {
            $scope.selectedAll1_PSFMAG = $scope.autoreceiptresult.every(function (itm) { return itm.Selected1_PSFMAG; })
        };

        $scope.chckedIndexs_PSFMAG = [];
        $scope.chckedIndexs1_PSFMAG = [];
        $scope.checkAll_PSFMAG = function () {
            if ($scope.autoreceipt.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PSFMAG;
                angular.forEach($scope.autoreceipt, function (itm) {
                    itm.Selected_PSFMAG = toggleStatus_PS;
                    if ($scope.chckedIndexs_PSFMAG.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PSFMAG.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PSFMAG.splice($scope.chckedIndexs_PSFMAG.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PSFMAG = false;
            }
        };

        $scope.checkAll1_PSFMAG = function () {

            if ($scope.autoreceiptresult.length > 0) {
                $scope.selectedAll1_PSFMAG = true;
                var toggleStatus_PS = $scope.selectedAll1_PSFMAG;
                angular.forEach($scope.autoreceiptresult, function (itm) {
                    itm.Selected1_PSFMAG = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PSFMAG.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PSFMAG.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PSFMAG.splice($scope.chckedIndexs1_PSFMAG.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PSFMAG = false;
            }
        };


        $scope.autoreceiptresult = [];
        $scope.GetFirstTablePSFMAG = function () {

            if ($scope.selectedAll_PSFMAG == true) {
                angular.forEach($scope.autoreceipt, function (student) {
                    $scope.autoreceiptresult.push(student);
                });
            } else {
                angular.forEach($scope.autoreceipt, function (student) {
                    if (student.Selected_PSFMAG == true) {
                        $scope.autoreceiptresult.push(student);
                    }
                });
            }
            $scope.autoreceipt = $scope.autoreceipt.filter(function (student) {
                return !student.Selected_PSFMAG
            })

            $scope.checkAll1_PSFMAG();
            $scope.checkAll_PSFMAG();
        };

        $scope.RemoveSecondTablePSFMAG = function () {
            if ($scope.selectedAll1_PSFMAG == true) {
                angular.forEach($scope.autoreceiptresult, function (student) {
                    $scope.autoreceipt.push(student);
                });
            } else {
                angular.forEach($scope.autoreceiptresult, function (student) {
                    if (student.Selected1_PSFMAG == true) {
                        $scope.autoreceipt.push(student);
                    }
                });
            }
            $scope.autoreceiptresult = $scope.autoreceiptresult.filter(function (student) {
                return !student.Selected1_PSFMAG;
            })
            $scope.checkAll_PSFMAG();
            $scope.checkAll1_PSFMAG();
        };

        $scope.OnChangeAcademicYearFMAG = function () {

            var data = {


                "ASMAY_Id": $scope.AMAY_Id_FMAG,
                "ASMAY_Order": $scope.NoOfYearsFMAG,

            }
            apiService.create("FeeWizard/changacademicyear", data).
                then(function (promise) {


                    $scope.enableautoreceiptentry = true;
                    $scope.AMAY_Id_PSFMAG1 = promise.academicyearnew[0].asmaY_Id;
                    $scope.yearListFMAG1 = promise.academicyearnew;
                    $scope.autoreceipt = promise.autoreceipt;

                })


        }


        $scope.saveautoreceiptdata = function () {

            $scope.finaldataPS = [];
            angular.forEach($scope.autoreceiptresult, function (student) {
                if (student.Selected1_PSFMAG == true) {
                    $scope.finaldataPS.push(student);
                }
            });
            if ($scope.finaldataPS.length > 0) {

                var plg = $scope.finaldataPS;
                var data = {
                    "ASMAY_Idnew": $scope.AMAY_Id_PSFMAG1,


                    resultData: plg
                }
                apiService.create("FeeWizard/savedetailFMAG", data).then(function (promise) {
                    if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                    }
                    else {
                        if (promise.message != null) {
                            swal('Record Updated Successfully', 'success');
                        }
                        else {
                            swal('Record Saved Successfully', 'success');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                swal('Kindly add atleast one record to second grid');
                return;
            }


        };

        $scope.deletefgar = function (det, SweetAlert) {

            var data = {
                //"FGARG_Id": det.fgarG_Id
                "FGAR_Id": det.fgaR_Id,
                "ASMAY_Id": det.asmaY_Id
            }

            swal({
                title: "Are you sure?",
                text: "All groups related to this receipt format will be deleted!!Do you want to Proceed?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeWizard/delete/", data).
                            then(function (promise) {

                                if (promise.returnduplicatestatus == "2") {
                                    swal("Sorry.....You can not delete this record.Because it is already mapped");
                                    return;
                                }
                                else if (promise.returnduplicatestatus === "1") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                }
                                else if (promise.returnduplicatestatus === "3") {
                                    swal('Receipt is already generated for this Format!!So Record cant be deleted.');
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }


        $scope.showmodaldetails = function (id) {

            var data = {
                "FGAR_Id": id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterGroupwiseAutoReceipt/printreceipt", data).
                then(function (promise) {
                    $scope.htmldata = promise.htmldata;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html($scope.htmldata))(($scope));
                })

        }

        $scope.Clearid_psFMAG = function () {

            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PSFMAG = "";
            $scope.NoOfYearsFMAG = "";

            $scope.selectedAll_PSFMAG = false;
            $scope.selectedAll1_PSFMAG = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PSFMAG = [];
            $scope.autoreceipt = [];

            $scope.autoreceiptresult = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];

            $scope.enableautoreceiptentry = false;
            $scope.AMAY_Id_FMAG = "";
            $scope.NoOfYearsFMAG = "";
            $scope.AMAY_Id_PSFMAG1 = "";

        };
        //Auto Receipt
    }
 


})();

