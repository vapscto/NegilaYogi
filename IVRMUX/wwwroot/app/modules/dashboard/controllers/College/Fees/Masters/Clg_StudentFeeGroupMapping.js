
(function () {
    'use strict';
    angular
.module('app')
.controller('Clg_StudentFeeGroupMappingController', Clg_StudentFeeGroupMappingController)

    Clg_StudentFeeGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function Clg_StudentFeeGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey1 = "acysT_RollNo";    //set the sortKey to the param passed
        $scope.sortReverse = true;      //if true make it false and vice versa
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};
        $scope.sort = function (keyname) {
            debugger;
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.sortReverse = !$scope.sortReverse; //if true make it false and vice versa
        }
        $scope.show_btn = false;
        $scope.show_cancel = false;
        $scope.show_grid = false;
        $scope.searc_button = true;

       


        var paginationformasters;
        $scope.page1 = "page1";
        $scope.page2 = "page2";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 3;
        }

        paginationformasters = 10;
        //=========For filter char count for first table===============//
        $scope.searchValue = '';
        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        //====================End================================//

        //=========For filter char count for Second table===============//
        $scope.searchValue1 = "";
        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        //====================End================================//

        //=======If any Semester checkboxlist select then SHOW button Display Other wise Not=======//


        $scope.toggleAll = function (allchkdata) {
            var toggleStatus = allchkdata;
            angular.forEach($scope.studentlist, function (itm) {
                itm.studchecked = toggleStatus;
            });
        }
        $scope.clar_sem = function () {
            
            $scope.chk_array = [];
            var chk_count = 0;
            angular.forEach($scope.semesterlist, function (itm) {
                if (itm.selected1 == true) {
                    chk_count += 1;
                    $scope.chk_array.push(itm);
                }
                if ($scope.chk_array.length > 0) {
                    $scope.show_btn = true;
                    $scope.show_cancel = true;

                }
                else {
                    $scope.show_btn = false;
                    $scope.show_cancel = false;

                    $scope.show_grid = false;
                }
            });
        }
        //====================End================================//


      
        $scope.search = "";
        $scope.show_flag = false;


        $scope.sort1 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        $scope.studentlist = [];


        //============Start Data Load on the Page==============//
        $scope.loaddata = function () {

            
            var pageid = 1;
            apiService.getURI("Clg_StudentFeeGroupMapping/GetYearList", pageid).
                then(function (promise) {
                    
                    $scope.yearlist = promise.yearlist;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.schemeType = promise.schemeType;
                    $scope.QuotaCategory = promise.termList;
                    $scope.get_courses();
                    // $scope.show_flag = false;
                })
        }
        //====================End===================//

        //===========Load ALL Courses data in to the CheckboxList===============//
        $scope.get_courses = function () {
            
            $scope.studentlist = [];
            $scope.show_flag = false;
            if ($scope.cfg.ASMAY_Id != undefined && $scope.cfg.ASMAY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }
                apiService.create("Clg_StudentFeeGroupMapping/get_courses", data).then(function (promise) {
                    $scope.courselist = promise.courselist;
                    // $scope.amcO_Id = "";
                    if ($scope.courselist.length == 0 || $scope.courselist == null) {
                        swal('For Selected Year Courses Are Not Available!!!');

                    }
                })
            }
            else {
                $scope.courselist = [];
                $scope.amcO_Id = "";
            }
            $scope.show_btn = false;
            $scope.show_cancel = false;

            $scope.show_grid = false;
        };
        //====================End===================//


        //================Load Branches data in to the CheckboxList=====================//
        $scope.get_branches = function () {
            
            $scope.studentlist = [];
            var idc = [];
            angular.forEach($scope.courselist, function (crs) {
                if (crs.selected) {
                    $scope.amcO_Id = crs.amcO_Id;
                    idc.push(crs.amcO_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                AMCO_Ids: idc
                //"ACAYC_Id": $scope.acaYC_Id
            }
            apiService.create("Clg_StudentFeeGroupMapping/get_branches", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
                // $scope.amB_Id = "";
                console.log($scope.branchlist)
                if ($scope.branchlist.length == 0 || $scope.branchlist == null) {
                    swal('For Selected Course Branches Are Not Available!!!');
                    $scope.show_btn = false;
                    $scope.show_cancel = false;
                    $scope.show_grid = false;
                }
            })
        }
        //========================END==================================//


        //============Load Semester data in to the CheckboxList==================//
        $scope.get_semisters = function () {

            $scope.studentlist = [];
            var idc = [];
            var idb = [];
            angular.forEach($scope.courselist, function (crs) {
                if (crs.selected) {
                    idc.push(crs.amcO_Id);
                }
            })
            angular.forEach($scope.branchlist, function (brc) {
                if (brc.selected) {
                    idb.push(brc.amB_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                AMCO_Ids: idc,
                AMB_Ids: idb
            }
            apiService.create("Clg_StudentFeeGroupMapping/get_semisters", data).
                then(function (promise) {
                    $scope.semesterlist = promise.semesterlist;
                    // $scope.amsE_Id = "";
                    if ($scope.semesterlist.length == 0 || $scope.semesterlist == null) {
                        swal('For Selected Branch Semesters Are Not Available!!!');
                        $scope.show_btn = false;
                        $scope.show_cancel = false;
                        $scope.show_grid = false;
                    }
                })
        };
        //==========================END============================//



        //====================Delete Record===================//
        $scope.DeleteRecord = function (user) {
            
            //var id = user.fcmsgH_Id
            //alert(id)
            var data = {
                "FCMSGH_Id": user.fcmsgH_Id
            }
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
                    
                    apiService.create("Clg_StudentFeeGroupMapping/DeleteRecord", data).
                        then(function (promise) {
                            if (promise.returnval == "true") {
                                swal('Record Deleted Successfully');
                            }
                            else if (promise.returnval == "false") {
                                swal('Record Not Deleted');
                            }
                            else if (promise.returnval == "Depend") {
                                swal('Record cannot be Deleted.Transaction has already  been done for this group');
                            }
                            $state.reload();
                        })
                }
                else {
                    swal("Record  Cancelled");
                }
            });
        }
        //====================End===================//



        //===========Load Student data in to the Table(grid)=============//
        $scope.get_report = function () {

            $scope.show_flag = false;
            $scope.submitted = true;
            $scope.currentPage = 1;
            $scope.currentPage2 = 1;

            $scope.itemsPerPage = paginationformasters;

            //$scope.studentlist = [];
            
            if ($scope.myForm.$valid) {
                
                var idc = [];
                var idb = [];
                var idse = [];
                angular.forEach($scope.courselist, function (crs) {
                    if (crs.selected) {
                        idc.push(crs.amcO_Id);
                    }
                })
                angular.forEach($scope.branchlist, function (brc) {
                    if (brc.selected) {
                        idb.push(brc.amB_Id);
                    }
                })
                angular.forEach($scope.semesterlist, function (sem) {
                    if (sem.selected1) {
                        idse.push(sem.amsE_Id);
                    }
                })

                var data = {
                    //  "MI_Id":$scope.MI_Id,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    AMCO_Ids: idc,
                    AMB_Ids: idb,
                    AMSE_Ids: idse,
                    "ACST_Id": $scope.ACST_Id,
                    "FOPM_Id":$scope.ACQ_Id
                }
                apiService.create("Clg_StudentFeeGroupMapping/get_report", data).then(function (promise) {

                    $scope.show_grid = true;
                    $scope.studentlist = promise.studentlist;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;

                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;
                    

                    $scope.StudentReport = promise.studentreport;

                    $scope.show_flag = true;
                    if (promise.studentlist != null) {
                        $scope.Recordlength = promise.studentlist.length;
                    }
                    if (promise.studentreport != null) {
                        $scope.Recordlength2 = promise.studentreport.length;
                    }
                    //$scope.amcsT_Id = "";
                    if ($scope.studentlist.length == 0 || $scope.studentlist == null) {
                        swal('For Selected details students are not Available!!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //======================End===========================//



        //======================Edit Record=========================//
        var eeditstudentsdata = [];
        $scope.edit = function (role) {
            
            var data = {

                "AMCST_Id": role.amcsT_Id,
                "ASMAY_Id": Number($scope.cfg.ASMAY_Id)
            }
            apiService.create("Clg_StudentFeeGroupMapping/editdata", data).then(function (promise) {
                    //role.studchecked = true;
                    $scope.AMCST_idedit = role.amcsT_Id;
                    $scope.eeditstudentsdata = promise.editdatalist;
                    if ($scope.eeditstudentsdata.length > 0) {
                        $scope.AMCST_idedit = promise.amcsT_Id;
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.disableins = false;
                        });

                        angular.forEach($scope.eeditstudentsdata, function (grpeditt) {

                            angular.forEach($scope.grouplstedit, function (objedit) {
                                if (grpeditt.fmG_Id == objedit.fmG_Id) {
                                    objedit.checkedgrplstedit = true;
                                    //if (grpeditt.fsS_PaidAmount > 0) {
                                    //    objedit.disablegrp = true;
                                    //}

                                    //objedit.disablegrp = true;
                                    angular.forEach($scope.headlstedit, function (objedit1) {
                                        if (grpeditt.fmG_Id == objedit1.fmG_Id && objedit1.fmH_Id == grpeditt.fmH_Id) {
                                            objedit1.checkedheadlstedit = true;
                                            //if (grpeditt.fsS_PaidAmount > 0) {
                                            //    objedit1.disablehead = true;
                                            //}

                                            angular.forEach($scope.installlstedit, function (objedit2) {
                                                if (grpeditt.fmG_Id == objedit2.fmG_Id && grpeditt.fmH_Id == objedit2.fmH_Id && grpeditt.ftI_Id == objedit2.ftI_Id) {
                                                    objedit2.checkedinstallmentlstedit = true;
                                                    if (grpeditt.fcsS_PaidAmount > 0) {
                                                        objedit2.disableins = true;
                                                        objedit1.disablehead = true;
                                                        objedit.disablegrp = true;
                                                    }
                                                }
                                            });
                                        }

                                    });
                                }

                            });
                        });
                    }
                    else {
                        angular.forEach($scope.grouplstedit, function (objedit) {
                            objedit.checkedgrplst = false;
                            objedit.disablegrp = false;
                        });
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            objedit1.checkedheadlst = false;
                            objedit1.disablehead = false;
                        });
                        angular.forEach($scope.installlstedit, function (objedit2) {
                            objedit2.checkedinstallmentlst = false;
                            objedit2.disableins = false;
                        });
                        swal("Student has not mapped with any of the group!")
                        $('#editmodal').modal('hide');
                        role.studchecked = false;
                        $scope.AMCST_idedit = 0;
                    }
                    $scope.isOptionsRequirededit1();
                })
        }
        //======================End===========================//


        //============================Save For GroupList Data=========================//
        $scope.valsgroup = [];
        $scope.valshead = [];
        $scope.valsinstallment = [];
        $scope.valstudentlst = [];
        $scope.unselected = [];
        //$scope.studentdata = promise.studentlist
        $scope.savedata = function (studentlist, grouplst, headlst, installlst) {
            
            $scope.valsgroup = [];
            $scope.valshead = [];
            $scope.valsinstallment = [];
            $scope.valstudentlst = [];
            $scope.page1 = "page1";
            
            if ($scope.myForm.$valid) {

              


                for (var w = 0; w < studentlist.length; w++) {
                    if (studentlist[w].studchecked == true) {
                        $scope.valstudentlst.push(studentlist[w]);
                    }
                }
                if ($scope.valstudentlst.length > 0)
                {
                    if ($scope.AMCST_idedit > 0) {
                        for (var t = 0; t < grouplst.length; t++) {
                            if (grouplst[t].checkedgrplstedit == true) {
                                $scope.valsgroup.push(grouplst[t]);
                            }
                            if (grouplst[t].checkedgrplstedit == false) {
                                $scope.unselected.push(grouplst[t]);
                            }
                        }

                        for (var u = 0; u < headlst.length; u++) {
                            if (headlst[u].checkedheadlstedit == true) {
                                $scope.valshead.push(headlst[u]);
                            }
                        }

                        for (var v = 0; v < installlst.length; v++) {
                            if (installlst[v].checkedinstallmentlstedit == true) {
                                $scope.valsinstallment.push(installlst[v]);
                            }
                        }
                    }
                    else {
                        for (var t = 0; t < grouplst.length; t++) {
                            if (grouplst[t].checkedgrplst == true) {
                                $scope.valsgroup.push(grouplst[t]);
                            }
                        }

                        for (var u = 0; u < headlst.length; u++) {
                            if (headlst[u].checkedheadlst == true) {
                                $scope.valshead.push(headlst[u]);
                            }
                        }

                        for (var v = 0; v < installlst.length; v++) {
                            if (installlst[v].checkedinstallmentlst == true) {
                                $scope.valsinstallment.push(installlst[v]);
                            }
                        }
                    }

                    //for (var w = 0; w < studentlist.length; w++) {
                    //    if (studentlist[w].studchecked == true) {
                    //        $scope.valstudentlst.push(studentlist[w]);
                    //    }
                    //}

                    studentlist = $scope.valstudentlst;
                    grouplst = $scope.valsgroup;
                    headlst = $scope.valshead;
                    installlst = $scope.valsinstallment;
                    if ($scope.valsgroup.length > 0)
                    {
                        var data = {
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            studentdata: studentlist,
                            savegrplst: grouplst,
                            saveheadlst: headlst,
                            saveftilst: installlst,
                            unselectedlist: $scope.unselected,

                        }

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("Clg_StudentFeeGroupMapping/savedata", data).
                        then(function (promise) {

                            if (promise.returnval == "false") {
                                swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.");
                            }
                            else if (promise.returnval == "duplicate") {
                                swal('Group is already saved for the student');
                            }
                            else if (promise.returnval == "true") {
                                swal('Record Saved Successfully');
                               // $state.reload();

                            }
                            else if (promise.returnval == "Error") {
                                swal('Kindly contact Administrator ');
                            }

                         //   $scope.cancel();
                            $state.reload();

                        })
                    }
                    else {
                        swal("Select Atleast One Group!!!");
                    }
                }
                else {
                    swal("Select Atleast One Student!!!");
                }
                
            }
            else {
                $scope.submitted = true;
            }
            $scope.cllose();
        };
        //======================End=======================//

        /////Added By praveen
        $scope.saveeditdata = function (grouplstedit, headlstedit, installlstedit) {
            var data = {
                "AMCST_Id": $scope.AMCST_idedit,
                savegrplst: grouplstedit,
                saveheadlst: headlstedit,
                saveftilst: installlstedit,
                "ASMAY_Id": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Clg_StudentFeeGroupMapping/saveeditdata", data).
                then(function (promise) {


                    if (promise.returnval == "false") {
                        swal("You are missing Amount entry/Due Date/Fine Slab Settings/Category Mapping.")
                    }
                    else if (promise.returnval == "true") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "Error") {
                        swal('Kindly contact Administrator ');
                    }     
                    //$state.reload();
                    $scope.formload();
                    $scope.cllose();
                });
        }
        /////



        //===================Cancel========================//
        $scope.cancel = function () {
           // $('#editmodal').modal('hide');
            $state.reload();
        }
        //===================End========================//


        //===========Field Validation=================//
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        //==================End===========================//


        //========Branchlist CheckBox Field Validation===========//
        $scope.isOptionsRequired_1 = function () {
            return !$scope.branchlist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========courselist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.courselist.some(function (options) {
                return options.selected;
            });
        }
        //==================End===========================//


        //========semesterlist CheckBox Field Validation============//
        $scope.isOptionsRequired_2 = function () {
            return !$scope.semesterlist.some(function (options) {
                return options.selected1;

            });
        }
        //==================End===========================//

        //==========Select for selected Grouplst,headlst,installment data for store record(for save button)================//
        $scope.firstfnc = function (vlobj) {
            
            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {
            
            if (vlobj1.checkedheadlst == true) {
                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = true;
                            }
                        });
                    }
                });
            } else {
                
                angular.forEach($scope.headlst, function (val) {
                    if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
                        angular.forEach($scope.installlst, function (val1) {
                            if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
                                val1.checkedinstallmentlst = false;
                            }
                        });
                    }
                });
            }
            
            for (var s = 0; s < $scope.grouplst.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplst[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplst[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfnc = function (vlobj2, oobjj) {
            for (var u = 0; u < $scope.headlst.length; u++) {
                if (vlobj2.fmG_Id == $scope.headlst[u].fmG_Id && vlobj2.fmH_Id == $scope.headlst[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlst.length; v++) {
                        if ($scope.installlst[v].fmH_Id == $scope.headlst[u].fmH_Id && $scope.installlst[v].fmG_Id == $scope.headlst[u].fmG_Id) {
                            if ($scope.installlst[v].checkedinstallmentlst == false) {
                                $scope.headlst[u].checkedheadlst = false;
                            }
                            else {
                                $scope.headlst[u].checkedheadlst = true;
                                break;
                            }
                        }
                    }
                    
                    for (var w = 0; w < $scope.grouplst.length; w++) {
                        if (vlobj2.fmG_Id == $scope.grouplst[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlst.length; x++) {
                                if (vlobj2.fmG_Id == $scope.headlst[x].fmG_Id) {
                                    if ($scope.headlst[x].checkedheadlst == false) {
                                        $scope.grouplst[w].checkedgrplst = false;
                                    }
                                    else {
                                        $scope.grouplst[w].checkedgrplst = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //==================End===========================//


        //==========Select for selected Grouplst,headlst,installment data for store record(for Edit)================//
        $scope.firstfncedit = function (vlobjedit) {

            if (vlobjedit.checkedgrplstedit == true) {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = true;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplstedit, function (objedit) {
                    if (vlobjedit.fmG_Id == objedit.fmG_Id) {
                        angular.forEach($scope.headlstedit, function (objedit1) {
                            if (objedit1.fmG_Id == objedit.fmG_Id) {
                                objedit1.checkedheadlstedit = false;
                                angular.forEach($scope.installlstedit, function (objedit2) {
                                    if (objedit.fmG_Id == objedit2.fmG_Id && objedit1.fmH_Id == objedit2.fmH_Id) {
                                        objedit2.checkedinstallmentlstedit = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfncedit = function (vlobjedit1) {
            
            if (vlobjedit1.checkedheadlstedit == true) {
                //angular.forEach($scope.grouplst, function (val) {
                //    if (vlobj1.fmG_Id == val.fmG_Id) {
                //        angular.forEach($scope.headlst, function (val1) {
                //            if (val1.fmG_Id == val.fmG_Id) {
                //                val1.checkedheadlst = true;
                //                angular.forEach($scope.installlst, function (val2) {
                //                    if (val.fmG_Id == val2.fmG_Id && val1.fmH_Id == val2.fmH_Id) {
                //                        val2.checkedinstallmentlst = true;
                //                    }
                //                });
                //            }
                //        });
                //    }
                //});

                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {

                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = true;
                            }

                        });
                    }
                });
            } else {
                angular.forEach($scope.headlstedit, function (valedit) {
                    if (vlobjedit1.fmG_Id == valedit.fmG_Id && vlobjedit1.fmH_Id == valedit.fmH_Id) {
                        angular.forEach($scope.installlstedit, function (valedit1) {
                            if (valedit1.fmH_Id == valedit.fmH_Id && valedit1.fmG_Id == valedit.fmG_Id) {
                                valedit1.checkedinstallmentlstedit = false;
                            }
                        });
                    }
                });
            }
            
            for (var s = 0; s < $scope.grouplstedit.length; s++) {
                if (vlobjedit1.fmG_Id == $scope.grouplstedit[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlstedit.length; t++) {
                        if (vlobjedit1.fmG_Id == $scope.headlstedit[t].fmG_Id) {
                            if ($scope.headlstedit[t].checkedheadlstedit == false) {
                                $scope.grouplstedit[s].checkedgrplstedit = false;
                            }
                            else {
                                $scope.grouplstedit[s].checkedgrplstedit = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.trdfncedit = function (vlobjedit2) {

            for (var u = 0; u < $scope.headlstedit.length; u++) {
                if (vlobjedit2.fmG_Id == $scope.headlstedit[u].fmG_Id && vlobjedit2.fmH_Id == $scope.headlstedit[u].fmH_Id) {
                    for (var v = 0; v < $scope.installlstedit.length; v++) {
                        if ($scope.installlstedit[v].fmH_Id == $scope.headlstedit[u].fmH_Id && $scope.installlstedit[v].fmG_Id == $scope.headlstedit[u].fmG_Id) {
                            if ($scope.installlstedit[v].checkedinstallmentlstedit == false) {
                                $scope.headlstedit[u].checkedheadlstedit = false;
                            }
                            else {
                                $scope.headlstedit[u].checkedheadlstedit = true;
                                break;
                            }
                        }
                    }
                    
                    for (var w = 0; w < $scope.grouplstedit.length; w++) {
                        if (vlobjedit2.fmG_Id == $scope.grouplstedit[w].fmG_Id) {
                            for (var x = 0; x < $scope.headlstedit.length; x++) {
                                if (vlobjedit2.fmG_Id == $scope.headlstedit[x].fmG_Id) {
                                    if ($scope.headlstedit[x].checkedheadlstedit == false) {
                                        $scope.grouplstedit[w].checkedgrplstedit = false;
                                    }
                                    else {
                                        $scope.grouplstedit[w].checkedgrplstedit = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //==================End===========================//


        //==============This is used for Field empty and close popup=====================//
        $scope.cllose = function () {
            $('#editmodal').modal('hide');
            angular.forEach($scope.grouplstedit, function (objedit) {
                objedit.checkedgrplstedit = false;
            });
            angular.forEach($scope.headlstedit, function (objedit1) {
                objedit1.checkedheadlstedit = false;
            });
            angular.forEach($scope.installlstedit, function (objedit2) {
                objedit2.checkedinstallmentlstedit = false;
            });
            angular.forEach($scope.studentlist, function (obj) {
                if (obj.amcsT_Id == $scope.AMCST_idedit) {
                    obj.studchecked = false;
                }
            });
            $scope.AMCST_idedit = 0;
           // $('#editmodal').modal('hide');
        }
        //==================End===========================//
        $scope.isOptionsRequired1 = function () {
            if ($scope.AMCST_idedit > 0) {
                return false;
            }
            else {
                return !$scope.grouplst.some(function (options) {
                    return options.checkedgrplst;
                });
            }
            
        }
        $scope.isOptionsRequirededit1 = function () {
            if ($scope.AMCST_idedit > 0)
            {
            return !$scope.grouplstedit.some(function (options) {
                    return options.checkedgrplstedit;
                });
            }
            else {
                return false;
            }
        }
    }
})();

