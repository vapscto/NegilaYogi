(function () {
    'use strict';
    angular.module('app').controller('CollegeTimetableSettingController', HallTicketCollegeReportController)

    HallTicketCollegeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$compile','$filter']
    function HallTicketCollegeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $compile, $filter) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.EXTTC_Id = 0;
        $scope.searchValue1 = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.items = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        $scope.imgname = logopath;
        $scope.printdata = false;
        $scope.gridlength = false;
        $scope.searchtext = "";
        $scope.searchValue1 = "";
        $scope.OnEditSubjects = [];
        $scope.BindData = function () {
            apiService.getDATA("HallTicketGenerationCollege/getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.course_list = promise.courseslist;
                $scope.alldata = promise.allDataTimeTable;
                $scope.ASMAY_Id = promise.asmaY_Id;


            });
        };
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.onselectBranch = function () {
            $scope.branch_list = [];
            $scope.AMB_Id = "";
            $scope.seclist = [];
            $scope.semisters_list = [];
            $scope.AMSE_Id = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("HallTicketGenerationCollege/onselectclass", data).then(function (promise) {
                if (promise.branchlist != null && promise.branchlist.length > 0) {
                    $scope.branch_list = promise.branchlist;
                }
                else {
                    swal("Record Not Found  !");
                }


            });
        };
       

        $scope.getsubjectschemetype = function () {
            $scope.schmetype_list = [];
            $scope.ACST_Id = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("ClgExamSubjectWizard/getsubjectschemetype", data).then(function (promise) {
                if (promise.schmetypelist != null && promise.schmetypelist.length > 0) {
                    $scope.schmetype_list = promise.schmetypelist;
                }
                
            });
        };
        $scope.get_subjects = function () {
            $scope.main_list = [];
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id
                //"EMG_Id": $scope.EMG_Id,
            }
            apiService.create("HallTicketGenerationCollege/HalticketSubject", data).then(function (promise) {
                if (promise.view_exam_subjects != null && promise.view_exam_subjects.length > 0) {
                    $scope.main_list = promise.view_exam_subjects;
                    $scope.time_slot = promise.time_slot;
                }
                else {
                    swal("Subjects Are Not  Mapped  !")
                }
                          
            });
        };

        $scope.OnchangeSemester = function () {
            $scope.seclist = [];
            $scope.semisters_list = [];
            $scope.AMSE_Id = "";
            $scope.exam_list = [];
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("HallTicketGenerationCollege/onselectAcdYear", data).then(function (promise) {
                if (promise.semisters != null && promise.semisters.length > 0) {
                    $scope.semisters_list = promise.semisters;
                }
                else {
                    swal("Record Not Found !");
                }

            });
        };

        $scope.get_exams = function () {
            $scope.exam_list = [];
            $scope.seclist = [];
            $scope.subjectschema_list = [];
            $scope.ACSS_Id = "";
            $scope.EME_Id = "";
            if ($scope.AMCO_Id !== "" && $scope.AMCO_Id !== undefined && $scope.AMSE_Id !== "" && $scope.AMSE_Id !== undefined && $scope.AMB_Id !== ""
                && $scope.AMB_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("HallTicketGenerationCollege/onselectsection", data).then(function (promise) {
                    if (promise.examlist != null && promise.examlist.length > 0) {
                        $scope.examlist = promise.examlist;
                        $scope.subjectschema_list = promise.subjectshemalist;
                    }
                    else {
                        swal("Exam Are Not Found !")
                    }
                    $scope.seclist = promise.sectionlist;
                });
            }
        };
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.ViewStudentDetails = function (viweobj) {


            $scope.studentlistnew = [];
            if ($scope.AMCO_Id !== "" && $scope.AMCO_Id !== undefined && $scope.AMSE_Id !== "" && $scope.AMSE_Id !== undefined && $scope.AMB_Id !== ""
                && $scope.AMB_Id !== undefined && $scope.EME_Id !== "" && $scope.EME_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "EME_Id": $scope.EME_Id,

                };

                apiService.create("HallTicketGenerationCollege/ViewStudentDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getStudent !== null && promise.getStudent.length > 0) {
                            $scope.studentlistnew = promise.getStudent;

                        } else {
                            swal("No Records Found");
                        }
                    }
                });

            }
            else {

            }

        };



        $scope.report = function () {
            $scope.selected_student = [];
            if ($scope.myForm.$valid) {
               
                $scope.arraylist = [];
                angular.forEach($scope.main_list, function (option) {
                    if (!!option.check_save) {
                        $scope.arraylist.push({
                            ISMS_Id: option.ismS_Id,
                            
                            EMTTSC_Id: option.ettS_Id,
                            EXTTSC_Date: new Date(option.exttS_Date).toDateString(),                                                     
                            EXTTC_ReportingTime: $filter('date')(option.ETTS_StartTime, "h:mm a")
                        });
                    }
                    
                })

                var data = {                   
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "EME_Id": $scope.EME_Id,
                    "EXTTC_ToDateTime": new Date($scope.myDate1).toDateString(),
                    "EXTTC_FromDateTime": new Date($scope.myDate2).toDateString(),
                    "EME_Id": $scope.EME_Id,
                    "TempararyArrayList": $scope.arraylist ,
                    "EXTTC_Id": $scope.EXTTC_Id ,
                    "ACST_Id": $scope.ACST_Id ,
                    "ACSS_Id": $scope.ACSS_Id ,
                }
                apiService.create("HallTicketGenerationCollege/savedetailHalticket", data).
                    then(function (promise) {
                        if (promise.returnval === true) {

                            swal('Data Saved successfully');
                            $scope.cancel();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }

                    });

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Toggle_header1 = function () {
            $scope.gridlength = false;
            $scope.printdata = false;
            var toggleStatus1 = $scope.all1;
            angular.forEach($scope.studentlistnew, function (itm) {
                itm.amsT_IdSelected = toggleStatus1;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.addColumn1 = function (role) {
            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.all1 = $scope.studentlistnew.every(function (itm) { return itm.amsT_IdSelected; });
            if (role.amsT_IdSelected === true) {
                $scope.albumNameArraycolumn.push(role);
                $scope.columnsTest.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.onclickdates = function () {
            $scope.gridlength = false;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.studentlistnew.some(function (options) {
                return options.amsT_IdSelected;
            });
        };
        //edit
        $scope.edit = function (user) {
            $scope.EXTTC_Id = user.EXTTC_Id;
            var data = {
                "ASMAY_Id": user.ASMAY_Id,
                "AMCO_Id": user.AMCO_Id,
                "AMSE_Id": user.AMSE_Id,
                "AMB_Id": user.AMB_Id,
                "EXTTC_Id": user.EXTTC_Id,
                "ACST_Id": user.ACST_Id,
                "ACSS_Id": user.ACSS_Id,
                "EME_Id": user.EME_Id,


            };
            apiService.create("HallTicketGenerationCollege/onedit", data).then(function (promise) {
                if (promise.view_exam_subjects != null && promise.view_exam_subjects.length > 0) {
                    $scope.main_list = promise.view_exam_subjects;
                    $scope.AMCO_Id = user.AMCO_Id;
                    $scope.ASMAY_Id = user.ASMAY_Id;
                    $scope.myDate1 = new Date(user.EXTTC_ToDateTime);
                    $scope.myDate2 = new Date(user.EXTTC_FromDateTime);
                    $scope.onselectBranch();
                    $scope.AMB_Id = user.AMB_Id;
                    $scope.OnchangeSemester();
                    $scope.AMSE_Id = user.AMSE_Id;
                    $scope.get_exams();
                    $scope.EME_Id = user.EME_Id;
                    $scope.ACSS_Id = user.ACSS_Id;
                    $scope.getsubjectschemetype();
                    $scope.ACST_Id = user.ACST_Id;
                    $scope.time_slot = promise.time_slot;
                    angular.forEach($scope.main_list, function (option) {

                        angular.forEach(promise.onEditSubjects, function (optiontwo) {
                            if (optiontwo.ismS_Id == option.ismS_Id) {                                                                  
                                option.exttS_Date = new Date(optiontwo.exttsC_Date);
                                option.ETTS_StartTime = moment(optiontwo.exttC_ReportingTime, 'h:mm a').format();
                                option.ettS_Id = optiontwo.emttsC_Id;
                                option.check_save =1;
                            }
                        })

                    })
                }
                else
                {
                    $scope.EXTTC_Id = 0;
                }
                
            });
        }
        $scope.printData = function () {
            if ($scope.dailybtedates === "daily") {
                var innerContents = document.getElementById("table").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            } else {
                var innerContents1 = document.getElementById("printformat1").innerHTML;
                var popupWinindow1 = window.open('');
                popupWinindow1.document.open();
                popupWinindow1.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
                popupWinindow1.document.close();
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

    }
})();