(function () {
    'use strict';
    angular.module('app').controller('HallTicketCollegeReportController', HallTicketCollegeReportController)

    HallTicketCollegeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$compile']
    function HallTicketCollegeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $compile) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.items = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        $scope.imgname = logopath;
        $scope.printdata = false;
        $scope.gridlength = false;
        $scope.searchtext = "";
        $scope.BindData = function () {
            apiService.getDATA("HallTicketGenerationCollege/getdetails").then(function (promise) {              
                $scope.acdlist = promise.acdlist;            
                $scope.course_list = promise.courseslist;
                $scope.ASMAY_Id = promise.asmaY_Id;
               

            });
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

                            $scope.section = promise.getStudent[0].ACMS_SectionName

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
            $scope.examname = "";
            if ($scope.myForm.$valid) {                       
                angular.forEach($scope.studentlistnew, function (dd) {
                    if (dd.amsT_IdSelected === true) {
                        $scope.selected_student.push({
                            AMCST_Id: dd.AMCST_Id,
                            EHTC_Id: dd.EHTC_Id
                        });
                    }
                });
                angular.forEach($scope.examlist, function (yyy) {
                    if (yyy.emE_Id === parseInt($scope.EME_Id)) {
                        $scope.examname = yyy.emE_ExamName;
                    }
                });
                //Exam_HallTicket_Generation_DATA
                var data = {
                    
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "section_Array": $scope.selected_student,
                    "FlagReport": $scope.dailybtedates
                };

                apiService.create("HallTicketGenerationCollege/ExamReport", data).then(function (promise) {

                    $scope.main_list = promise.getStudent;
                    if ($scope.main_list !== null && $scope.main_list.length > 0) {
                        $scope.gridlength = true;
                        $scope.print = false;
                        $scope.printdata = true;
                        if ($scope.dailybtedates == "btwdates") {
                           // if (promise.htmldata !== null && promise.htmldata !== "") {

                                
                            $scope.employeeid = [];
                     
                                   angular.forEach($scope.main_list, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push(dev);
                                    } else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.AMCST_Id === dev.AMCST_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push(dev);
                                        }
                                    }
                            }); 

                            $scope.acayear = $scope.employeeid[0].ASMAY_Year;
                                console.log($scope.employeeid);
                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];                                   
                                    angular.forEach($scope.main_list, function (dd) {
                                        if (dd.AMCST_Id === ddd.AMCST_Id) {
                                            
                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.plannerdetails = $scope.templist;
                                   
                                });


                                console.log($scope.employeeid);

                                var e1 = angular.element(document.getElementById("report"));
                                $scope.dynamichtml = true;
                                $compile(e1.html(promise.htmldata))(($scope));
                           // }

                        }
                        else {
                            angular.forEach($scope.main_list, function (dd) {
                                var str = dd.EHTC_HallTicketNo;
                                dd.arr3 = new Array(...str);
                            });

                            $scope.configuraion = promise.configuraion;

                            $scope.studentlist = promise.studarray;
                            $scope.sublist = promise.subarray;

                            $scope.intitutelist = promise.institute;

                            angular.forEach($scope.acdlist, function (yy) {
                                if (yy.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.accyear = yy.asmaY_Year;
                                }
                            });

                            

                            $scope.dynamichtml = false;

                        }
                         
                        if ($scope.configuraion.length > 0) {
                            $scope.principal = $scope.configuraion[0].ivrmgC_PrincipalSign;
                        }
                        else {
                            $scope.principal = "";
                        }
                    } else {
                        swal("No Records Found");
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
                //popupWinindow1.document.write('<html><head>' +
                //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                //    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');

                //popupWinindow1.document.close();
                popupWinindow1.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
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