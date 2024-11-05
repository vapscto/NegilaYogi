
(function () {
    'use strict';

    angular
        .module('app')
        .controller('VAC132Mapping', VAC132Mapping);

    VAC132Mapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function VAC132Mapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {


        //======================for pagination
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.NCACVAC132D_Date = new Date();

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        ///======================================Load Data.
        $scope.NCACVAC132_Id = 0;
        $scope.institute_flag = false;
        $scope.loaddata = function () {
            $scope.change_instload();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("Naac_VAC/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.introyearlist = promise.introyearlist;
                $scope.discontyearlist = promise.discontyearlist;
                $scope.detailsyearlist = promise.detailsyearlist;
                $scope.completedyearlist = promise.completedyearlist;
                $scope.ccourseNamelist = promise.ccourseNamelist;
                $scope.alldata = promise.alldata;
                $scope.ccourseNamelist = promise.ccourseNamelist;
                $scope.alldatatab2 = promise.alldatatab2;
                $scope.reportlist = promise.reportlist;
                $scope.studentlist1 = promise.studentlist1;
              
                
                angular.forEach($scope.alldatatab2, function (vc) {
                    var q = 0;
                    angular.forEach($scope.studentlist1, function (vcc) {                      
                        if (vc.ncacvaC132D_Id == vcc.ncacvaC132D_Id) {
                             q = q + 1;
                        }                        
                    })
                    vc.count1 = q;                  
                })
            })
        }

        //=====================Sorting
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //==============================save data For Tab1

        $scope.savedatatab1 = function () {
            $scope.studentlstdata = [];
            if ($scope.myForm.$valid) {
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "NCACVAC132_Id": $scope.NCACVAC132_Id,
                        "NCACVAC132_IntroYear": $scope.ASMAY_Id,
                        "NCACVAC132_CourseCode": $scope.NCACVAC132_CourseCode,
                        "NCACVAC132_CourseName": $scope.NCACVAC132_CourseName,
                        "filelist": $scope.materaldocuupload,
                        "MI_Id": $scope.mI_Id,
                    }
                }

                apiService.create("Naac_VAC/savedatatab1", data).then(function (promise) {
                    if (promise.duplicate != null && promise.returnval != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.NCACVAC132_Id > 0) {
                                    swal('Data Updated Successfully!')
                                }
                                else {
                                    swal('Data Saved Successfully!')
                                }

                                $state.reload();
                            }
                            else {
                                if ($scope.NCACVAC132_Id > 0) {
                                    swal('Data Not Updated Successfully!')
                                }
                                else {
                                    swal('Data Not Saved Successfully!')
                                }
                            }
                        }
                        else {
                            swal('Record already Exist!')
                        }
                    }
                })

            }
            else {
                $scope.submitted = true;
            }
        };

        //==========================edit data for tab1
        $scope.edittab1 = function (data) {
            apiService.create("Naac_VAC/edittab1", data).then(function (promise) {
                $scope.institute_flag = true;
                $scope.editlisttab1 = promise.editlisttab1;
                $scope.mI_Id = promise.editlisttab1[0].mI_Id;
                $scope.NCACVAC132_Id = promise.editlisttab1[0].ncacvaC132_Id;
                $scope.NCACVAC132_CourseName = promise.editlisttab1[0].ncacvaC132_CourseName;
                $scope.NCACVAC132_CourseCode = promise.editlisttab1[0].ncacvaC132_CourseCode;
                $scope.ASMAY_Id = promise.editlisttab1[0].ncacvaC132_IntroYear;

                $scope.materaldocuupload = promise.editMainSActFileslist;
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (ddd) {
                        var img = ddd.cfilepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                        }
                    })
                }
                else {
                    $scope.materaldocuupload = [{ id: 'Materal1' }];
                }

            })
        }

        //===========deactive and active for Tab1
        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            $scope.NCACVAC132_Id = usersem.ncacvaC132_Id
            var dystring = "";
            if (usersem.ncacvaC132_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacvaC132_ActiveFlg == false) {
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
                        apiService.create("Naac_VAC/deactivYTab1", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d " + "Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==================filter Name
        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {
            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ncacsP123_AddOnProgramName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //========Studentlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (item) {
                return item.selected;
            });
        }

        //=======selection of checkbox....
        $scope.NCACVAC132D_NoOfStudentsEnr = 0;
        $scope.togchkbxC = function (SelectedStudentRecord) {

            $scope.usercheckC = $scope.studentlist.every(function (itm) { return itm.selected; });
            if (SelectedStudentRecord.selected == true) {
                $scope.NCACVAC132D_NoOfStudentsEnr += 1;
            } else {
                $scope.NCACVAC132D_NoOfStudentsEnr -= 1;
            }
        }

        //---------all checkbox Select...
        $scope.all_checkC = function (all) {
            $scope.NCACVAC132D_NoOfStudentsEnr = 0;
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.studentlist, function (role) {
                //role.selected = toggleStatus;
                if (role.selected = toggleStatus) {
                    $scope.NCACVAC132D_NoOfStudentsEnr += 1;
                }
            });
        }

        //====================================Get Student list for Tab2
        $scope.Get_student = function () {
            $scope.searchchkbx1 = "";
            $scope.usercheckC = "";
            $scope.NCACVAC132D_NoOfStdCompleted = 0;
            $scope.NCACVAC132D_NoOfStudentsEnr = 0;
            $scope.studentlist = [];
            var data = {
                "ASMAY_Id": $scope.NCACVAC132D_Year,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("Naac_VAC/get_student", data).then(function (promise) {

                $scope.studentlist = promise.studentlist;
                $scope.ccourseNamelist = promise.ccourseNamelist;
            })
        }

        //================================Cancel for tab2
        $scope.canceltab2 = function () {
            $state.reload();
            $scope.institute_flag = false;
            $scope.searchchkbx1 = false;
            $scope.edit_falgdata = false;
            $scope.NCACVAC132_Id = "";
            $scope.NCACVAC132D_Id = "";
            $scope.NCACVAC132D_Date = new Date();
            $scope.NCACVAC132D_Year = "";
            $scope.NCACVAC132D_NoOfStudentsEnr = 0;
            $scope.usercheckC = false;
            $scope.studentlist = [];
            //angular.forEach($scope.studentlist, function (itm1) {
            //    itm1.selected = false;
            //})
            $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];
            $scope.submitted2 = false;
            $scope.submitted1 = false;
            $scope.submitted = false;
        }

        ////=============================for tab2
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        //==============================save data For Tab2
        $scope.totalStudentflag = true;
        $scope.submitted2 = false;
        $scope.Savedatatab2 = function () {
   
            $scope.studentlstdata = [];
            if ($scope.myForm2.$valid) {
                angular.forEach($scope.studentlist, function (std) {
                    if (std.selected == true) {
                        $scope.studentlstdata.push({ AMCST_Id: std.amcsT_Id });
                    }
                });

                var entrydate = $scope.NCACVAC132D_Date == null ? "" : $filter('date')($scope.NCACVAC132D_Date, "yyyy-MM-dd");

                var data = {
                    "NCACVAC132D_Id": $scope.NCACVAC132D_Id,
                    "NCACVAC132DS_Id": $scope.NCACVAC132DS_Id,
                    "NCACVAC132_Id": $scope.NCACVAC132_Id,
                    "NCACVAC132D_Date": entrydate,
                    "NCACVAC132D_Year": $scope.NCACVAC132D_Year,
                    "NCACVAC132D_NoOfStudentsEnr": $scope.NCACVAC132D_NoOfStudentsEnr,
                    studentlstdata: $scope.studentlstdata,
                    "filelist_student": $scope.materaldocuuploadStudent,
                    "MI_Id": $scope.mI_Id,
                }


                apiService.create("Naac_VAC/savedatatab2", data).then(function (promise) {
                    if (promise.returnval != null) {
                        if (promise.returnval == true) {
                            if ($scope.NCACVAC132D_Id > 0) {
                                swal('Data Updated Successfully...!!!')
                            }
                            else {
                                swal('Data Saved Successfully...!!!')
                            }

                            $state.reload();
                        }
                        else {
                            if ($scope.NCACVAC132D_Id > 0) {
                                swal('Data Not Updated Successfully!')
                            }
                            else {
                                swal('Data Not Saved Successfully!')
                            }
                        }
                        //}
                        //else {
                        //    swal('Record Already Exists!')
                        //}
                    }
                })
            }
            else {
                $scope.submitted2 = true;
            }
        };

        //=======================================End

        // for comment
        $scope.getorganizationdata = function (obj) {
          
            apiService.create("Naac_VAC/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view student
        $scope.getorganizationdatastudent = function (obj) {
      
            apiService.create("Naac_VAC/viewstudent", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.viewstudentlist = promise.viewstudentlist;
                }
            });
        };

        // for file 
        $scope.getorganizationdata1 = function (obj) {
    
            apiService.create("Naac_VAC/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
        
            $scope.ccc = obje.ncacvaC132D_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
         
            $scope.cc = obje.ncacvaC132DF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** Save DATA WISE Comments ***************//
        $scope.savedatawisecomments = function (obj) {
        
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("Naac_VAC/savemedicaldatawisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $('#mymodalviewuploaddocument').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };

        // view student
        //$scope.savedatawisecomments = function (obj) {
     
        //    console.log("Save Comments");
        //    console.log(obj);
        //    var data = {
        //        "Remarks": obj.generalcomments,
        //        "filefkid": $scope.ccc
        //    };
        //    apiService.create("Naac_VAC/savemedicaldatawisecommentsstudent", data).then(function (promise) {
        //        if (promise !== null) {
        //            if (promise.returnval === true) {
        //                swal("Comments Saved Successfully");
        //            } else {
        //                swal("Failed To Save Comments");
        //            }
        //            $('#mymodaladdcommentsstudent').modal('hide');
        //            $('#mymodalviewuploaddocumentstudent').modal('hide');
        //            $scope.valued = "2";
        //            $scope.onload();
        //        }
        //    });
        //};

        // fr add file comment 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("Naac_VAC/savefilewisecomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments1').modal('hide');
                    $('#mymodalviewuploaddocument1').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };


        //========================Check Field Value
        $scope.NCACVAC132D_NoOfStdCompleted = 0;
        $scope.check_fielvalue = function () {
            if (($scope.NCACVAC132D_NoOfStudentsEnr != null) || ($scope.NCACVAC132D_NoOfStudentsEnr != undefined) || ($scope.NCACVAC132D_NoOfStudentsEnr != "")) {
                if ($scope.NCACVAC132D_NoOfStudentsEnr >= $scope.NCACVAC132D_NoOfStdCompleted) {

                }
                else {
                    swal('You can not give more then value from No Of Student Entry!');
                    $scope.NCACVAC132D_NoOfStdCompleted = "";
                }
            }
        }




        //===================edit tab2
        $scope.edit_falgdata = false;
        $scope.edittab2 = function (user) {
          
            apiService.create("Naac_VAC/edittab2", user).then(function (promise) {
            
                $('#mymodalviewuploaddocumentstudent').modal('hide');
                $scope.edit_falgdata = true;
                $scope.institute_flag = true;

               
                $scope.ncacvaC132D_StatusFlg = promise.editlisttab2[0].ncacvaC132D_StatusFlg;
                if ($scope.ncacvaC132D_StatusFlg == 'approved') {
                    $scope.dis = true;
                }
                else {
                    $scope.dis = false;
                }
                $scope.editlisttab2 = promise.editlisttab2;
                $scope.mI_Id = promise.editlisttab2[0].mI_Id;
                $scope.NCACVAC132D_Id = promise.editlisttab2[0].ncacvaC132D_Id;
                $scope.NCACVAC132DS_Id = promise.editlisttab2[0].ncacvaC132DS_Id;
                //$scope.NCACVAC132DS_Id = promise.editlisttab2[0].ncacvaC132DS_Id;
                $scope.NCACVAC132_Id = promise.editlisttab2[0].ncacvaC132_Id;
                $scope.NCACVAC132D_Year = promise.editlisttab2[0].ncacvaC132D_Year;
                $scope.NCACVAC132D_NoOfStudentsEnr = promise.editlisttab2[0].ncacvaC132D_NoOfStudentsEnr;
                $scope.NCACVAC132D_NoOfStdCompleted = promise.editlisttab2[0].ncacvaC132D_NoOfStdCompleted;

                if (promise.editlisttab2[0].ncacvac132D_Date != null || promise.editlisttab2[0].ncacvac132D_Date != undefined || promise.editlisttab2[0].ncacvac132D_Date != "") {
                    $scope.NCACVAC132D_Date = new Date(promise.editlisttab2[0].ncacvaC132D_Date);
                }
                $scope.studentlist = promise.studentlist;
                $scope.studentedit = promise.studentedit;
                angular.forEach($scope.studentlist, function (ss) {
                    angular.forEach(promise.studentedit, function (tt) {
                        if (ss.amcsT_Id == tt.amcsT_Id) {
                            ss.selected = true;
                        }
                    })
                })
                $scope.materaldocuuploadStudent = promise.editStudentActFileslist;
                if ($scope.materaldocuuploadStudent.length > 0) {
                    angular.forEach($scope.materaldocuuploadStudent, function (ddd) {
                        var img = ddd.cfilepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                        }
                    })
                }
                else {
                    $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];
                }
                $('#mymodalviewuploaddocumentstudent').modal('hide');
                


            })
        }

        //===========deactive and active for Tab2
        $scope.deactivYTab2 = function (usersem, SweetAlert) {
            //$scope.NCACVAC132DS_Id = usersem.ncacvaC132DS_Id
            var dystring = "";
            if (usersem.ncacvaC132D_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacvaC132D_ActiveFlg == false) {
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
                        apiService.create("Naac_VAC/deactivYTab2", usersem).
                            then(function (promise) {
                               
                                if (promise.message == 'cantdeact') {
                                    swal("Students are Mapped...You can't De-Activate the Record");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record " + dystring + "d" + " Successfully!!!");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    }
                                }
                               
                                //$scope.alldatatab2 = promise.alldatatab2;
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        $scope.deactivYTabstudent = function (usersem, SweetAlert) {
            //$scope.NCACVAC132DS_Id = usersem.ncacvaC132DS_Id
            var dystring = "";
            if (usersem.ncacvaC132DS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacvaC132DS_ActiveFlg == false) {
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
                        apiService.create("Naac_VAC/deactivYTabstudent", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $scope.viewstudentlist = promise.viewstudentlist;
                                    $('#mymodalviewuploaddocumentstudent').modal('view');
                                  
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                }
                               // $('#mymodalviewuploaddocumentstudent');
                               
                            })
                        //$('#mymodalviewuploaddocumentstudent').modal('view');
                        //$state.reload();
                         
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }



        //=============================Get mapped student List

        $scope.get_Mappedstudentlist = function (obj1) {
            apiService.create("Naac_VAC/get_Mappedstudentlist", obj1).then(function (promise) {
                $scope.mappedstudentlist = promise.mappedstudentlist;
            })
        }
        //===============Excel Sheet
        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 1.3.2 xls';
            var exportHref = Excel.tableToExcel(table, '1.3.2');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        $scope.ClearidModal = function () {
            $scope.NCACVAC132_DiscontinuedYear = "";
            $scope.submitted3 = false;
        }
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.get_Continuedflagdata1 = function (user) {
        
            var data = {
                "NCACVAC132_Id": user.ncacvaC132_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("Naac_VAC/get_Continuedflagdata", data).then(function (promise) {

                $scope.discontyearlist = promise.discontyearlist;
                $scope.get_Continuedflagdata = promise.get_Continuedflagdata;
                $scope.ncacvaC132_Id = promise.get_Continuedflagdata[0].ncacvaC132_Id;
            })
        }
        //==================save only For discontinued
        $scope.submitted3 = false;
        $scope.saveContinued = function () {
            if ($scope.myFormModal.$valid) {
                var data = {
                    "NCACVAC132_Id": $scope.ncacvaC132_Id,
                    "NCACVAC132_DiscontinuedYear": $scope.NCACVAC132_DiscontinuedYear,
                    "MI_Id": $scope.mI_Id,
                }
                apiService.create("Naac_VAC/saveContinued", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else {
                        swal('Record Not Saved Successfully');
                    }
                })
            }
            else {
                $scope.submitted3 = true;
            }
        }
        //========================================
        $scope.submitted4 = false;
        $scope.ClearidModal2 = function () {
            $scope.NCACVAC132DS_CompletedYear = "";
            $scope.submitted4 = false;
        }
        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };
        //===================get Completed flag data  
        $scope.comp_falgdata = false;
        $scope.get_Completedflagdata = function (user) {

            var data = {
                "NCACVAC132_Id": user.ncacvaC132_Id,
                "NCACVAC132DS_Id": user.ncacvaC132DS_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("Naac_VAC/get_Completedflagdata", data).then(function (promise) {

                $scope.completedyearlist = promise.completedyearlist;
                $scope.countOfStudentEntry = promise.countOfStudentEntry;
                $scope.studentlist = promise.studentlist;
                if (promise.countOfStudentEntry.length > 0) {
                    $scope.comp_falgdata = true;
                    $scope.NCACVAC132D_NoOfStudentsEnr = promise.countOfStudentEntry[0].ncacvaC132D_NoOfStudentsEnr;
                    $scope.ncacvaC132D_Id = promise.countOfStudentEntry[0].ncacvaC132D_Id;
                    $scope.ncacvaC132DS_Id = promise.countOfStudentEntry[0].ncacvaC132DS_Id;
                    $scope.amcsT_Id = promise.countOfStudentEntry[0].amcsT_Id;
                    $scope.NCACVAC132D_NoOfStudentsEnr = promise.countOfStudentEntry[0].ncacvaC132D_NoOfStudentsEnr;
                    var count = 0;
                    angular.forEach($scope.studentlist, function (totalstd) {
                        if (totalstd.amcsT_Id == $scope.amcsT_Id) {
                            totalstd.selected = true;
                            count += 1;
                        }
                    })
                    $scope.NCACVAC132D_NoOfStdCompleted = count;
                }
            })
        }
        $scope.saveCompletedflag = function () {

            $scope.studentlstdata = [];
            if ($scope.myForm4.$valid) {
                angular.forEach($scope.studentlist, function (std) {
                    if (std.selected == true) {
                        $scope.studentlstdata.push({ AMCST_Id: std.amcsT_Id });
                    }
                });
                var data = {
                    "NCACVAC132D_Id": $scope.ncacvaC132D_Id,
                    "NCACVAC132DS_Id": $scope.ncacvaC132DS_Id,
                    "NCACVAC132D_NoOfStdCompleted": $scope.NCACVAC132D_NoOfStdCompleted,
                    "NCACVAC132DS_CompletedYear": $scope.NCACVAC132DS_CompletedYear,
                    "studentlstdata": $scope.studentlstdata,
                }
                apiService.create("Naac_VAC/saveCompletedflag", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record is Updated Successfully!");
                        $state.reload();
                    }
                    else {
                        swal("Record is Not Updated Successfully!");
                    }
                })
            }
            else {
                $scope.submitted4 = true;
            }
        }
        // ==============Main Activity=======================================//
        //===============================view document files
        $scope.viewdocument_MainActUploadFiles = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("Naac_VAC/viewuploadfliesmain", obj).then(function (promise) {

                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.viewuploadfliesmain;
                    if (promise.viewuploadfliesmain !== null && promise.viewuploadfliesmain.length > 0) {
                        $scope.uploaddocfiles = promise.viewuploadfliesmain;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete document files
        $scope.delete_MainActUploadFiles = function (docfile) {

            var data = {
                "NCACVAC132_Id": docfile.ncacvaC132_Id,
                "NCACVAC132F_Id": docfile.ncacvaC132F_Id,
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Naac_VAC/deletemainfile", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Deleted successfully");

                                $scope.viewuploadflies = promise.deletemainfile;
                                $scope.uploaddocfiles = promise.viewuploadflies;
                                if (promise.deletemainfile !== null && promise.deletemainfile.length > 0) {
                                    $scope.uploaddocfiles = promise.deletemainfile;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                } else {
                                    $scope.uploaddocfiles = [];
                                }
                            }
                        })
                    }
                    else {
                        swal("Record Deletation Cancelled!!!");
                    }
                });
        }
        //================================================


        //=============================Activity upload
        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };



        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {
          
            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("size should be less than 2MB");
                    return;
                }
                else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdf').modal('show');
                });
        };

        //==================Student Activity Upload============================//   
        $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];

        $scope.addmateralstudent = function () {
            var newItemNo = $scope.materaldocuuploadStudent.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuuploadStudent.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateralstudent = function (index) {
            var newItemNo = $scope.materaldocuuploadStudent.length - 1;
            $scope.materaldocuuploadStudent.splice(index, 1);

            if ($scope.materaldocuuploadStudent.length === 0) {
                //data
            }
        };

        $scope.showmothersignstudent = function (path) {
            $('#previewstudent').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimgstudent').modal('show');
        };

        $scope.showGuardianPhotonewStudent = function (data) {
            $scope.view_student_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_student_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_student_videos);
        };

        $scope.onviewstudent = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.contentstudent = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.contentstudent = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdfstudent').modal('show');
                });
        };

        //===============================view Student document files
        $scope.viewdocument_StudentActUploadFiles = function (obj) {
           
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("Naac_VAC/viewuploadfliesstudent", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.viewuploadfliesstudent;
                    if (promise.viewuploadfliesstudent !== null && promise.viewuploadfliesstudent.length > 0) {
                        $scope.uploaddocfiles = promise.viewuploadfliesstudent;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup12').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete Student document files       
        $scope.delete_StudentActUploadFiles = function (docfile) {
         
            var data = {
                "NCACVAC132DF_Id": docfile.ncacvaC132DF_Id,
                "NCACVAC132D_Id": docfile.ncacvaC132D_Id,
            }
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Naac_VAC/deletestudentfiles", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");

                                $scope.viewuploadflies = promise.viewuploadfliesstudent;
                                $scope.uploadfilesdetails = $scope.viewuploadflies;
                                if (promise.viewuploadfliesstudent !== null && promise.viewuploadfliesstudent.length > 0) {
                                    $scope.uploaddocfiles = promise.viewuploadfliesstudent;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }
                                else {
                                    $scope.uploaddocfiles = [];
                                }
                            }
                            else {
                                swal("Record Deletion Failed");
                            }


                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };
        //================================================

        $scope.change_instload = function () {

            $scope.ASMAY_Id = "";
            $scope.NCACVAC132_Id = 0;
            $scope.NCACVAC132D_Id = 0;
            $scope.NCACVAC132_CourseName = "";
            $scope.NCACVAC132_CourseCode = "";
            $scope.NCACVAC132D_Year = "";
            $scope.NCACVAC132D_Date = new Date();
            $scope.studentlist = [];
            $scope.searchchkbx1 = "";
            $scope.usercheckC = "";
            $scope.NCACVAC132D_NoOfStudentsEnr = 0;
            $scope.materaldocuuploadStudent = [];
            $scope.materaldocuupload = [];
            $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];
            $scope.materaldocuupload = [{ id: 'Materal1' }];


        }

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();