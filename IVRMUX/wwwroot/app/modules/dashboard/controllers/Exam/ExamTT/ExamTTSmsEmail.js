(function () {
    'use strict';
    angular
.module('app')
.controller('ExamTTSmsEmailController', ExamTTSmsEmailController)

    ExamTTSmsEmailController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', '$q', '$stateParams' ,'$window']
    function ExamTTSmsEmailController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, $q, $stateParams, $window) {



        $scope.BindData = function () {
            apiService.getDATA("ExamTTSmsEmail/getdetails").
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.examlist = promise.examlist;
            })
        };
        $scope.submitted = false;
        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamTTSmsEmail/onselectAcdYear", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamTTSmsEmail/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.onselectSection = function () {
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("ExamTTSmsEmail/onselectSection", data).
            then(function (promise) {
                $scope.subject_group = promise.subject_group;
                $scope.examlist = promise.examlist;

            })
        };

        $scope.getStudentsTeachers = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id
                }
                apiService.create("ExamTTSmsEmail/getStudentsTeachers", data).
                then(function (promise) {
                    $scope.main_list = promise.studentlist;
                    $scope.main_list1 = promise.teacherlist;
                })
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.generate = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    //"EMG_Id": $scope.EMG_Id,
                }

                apiService.create("ExamTTSmsEmail/generate", data).
                then(function (promise) {
                    
                    $scope.main_list2 = promise.generateTT;
                })
            } else {
                $scope.submitted = true;
            }
        };


        $scope.exptoex = function () {
            var divToPrint = document.getElementById("timetable");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            var a = saveAs(blob, "Report.xls");
            $scope.url = (window.URL || window.webkitURL).createObjectURL(blob);

        };

        $scope.emailflag = false;


        $scope.sendmail = function () {

            // $scope.exptoex();
            $scope.student_array = [];
            angular.forEach($scope.main_list, function (optionstd) {
                if (!!optionstd.check_save_stud)
                    $scope.student_array.push(optionstd);
            });

            $scope.teacher_array = [];
            angular.forEach($scope.main_list1, function (option) {
                if (!!option.check_save_teach)
                    $scope.teacher_array.push(option);
            });
            
            if ($scope.student_array.length > 0 || $scope.teacher_array.length > 0) {
                if ($scope.myForm.$valid) {


                    var data = {
                        "ASMS_Id": $scope.ASMS_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EME_Id": $scope.EME_Id,
                        "EMG_Id": $scope.EMG_Id,
                        "check_save_studdto": $scope.student_array,
                        "check_save_teachdto": $scope.teacher_array,
                        "url": $scope.url
                    };
                    apiService.create("ExamTTSmsEmail/sendmail", data).
                        then(function (promise) {
                            swal('Email and SMS sent Successfully');
                            $state.reload();
                        });

                }
            }
            else {
                swal("Please select checkbox");
            }

        };

        $scope.sendmailss = function () {

            // $scope.exptoex();
            $scope.student_array = [];
            angular.forEach($scope.main_list, function (optionstd) {
                if (!!optionstd.check_save_stud)
                    $scope.student_array.push(optionstd);
            });

            $scope.teacher_array = [];
            angular.forEach($scope.main_list1, function (option) {
                if (!!option.check_save_teach)
                    $scope.teacher_array.push(option);
            });

            if ($scope.student_array.length > 0 || $scope.teacher_array.length > 0) {
                if ($scope.myForm.$valid) {
                    var data = {
                        "ASMS_Id": $scope.ASMS_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EME_Id": $scope.EME_Id,
                        "EMG_Id": $scope.EMG_Id,
                        "check_save_studdto": $scope.student_array,
                        "check_save_teachdto": $scope.teacher_array,
                        "url": $scope.url,
                        "emailflag": true,
                        "smsflag": false
                    };
                    apiService.create("ExamTTSmsEmail/sendmail", data).
                        then(function (promise) {
                            swal('SMS & Email sent Successfully');
                        });
                }
            }
            else {
                swal("Please select checkbox");
            }

        };

        $scope.all_check = function () {
            
            var toggleStatus = $scope.check_all;
            angular.forEach($scope.main_list, function (itm) {
                itm.check_save_stud = toggleStatus;
            });
        }
        $scope.addColumn1 = function (role) {
            $scope.check_all = $scope.main_list.every(function (itm)

            { return itm.check_save_stud; });
        }


        $scope.all_check1 = function () {
            
            var toggleStatus = $scope.check_all1;
            angular.forEach($scope.main_list1, function (itm) {
                itm.check_save_teach = toggleStatus;
            });
        }
        $scope.addColumn = function (role) {
            $scope.check_all1 = $scope.main_list1.every(function (itm)

            { return itm.check_save_teach; });
        }
        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };


        $scope.getsubjectgroup = function () {
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id,
            }
            apiService.create("ExamTTSmsEmail/getsubjectgroup", data).then(function (promise) {

                if (promise != null) {
                    if (promise.subject_group != null) {
                        $scope.subject_group = promise.subject_group;
                    } else {
                        swal("No Records Found")
                    }
                } else {
                    swal("No Records Found")
                }
            })
        }



        $scope.pdf = function () {
            

            var doc = document.getElementById("timetable");
            var doc1 = doc.innerHTML;

            html2canvas(document.getElementById("timetable"), {
                onrendered: function (canvas) {

                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            styles: {
                                header: {
                                    fontSize: 50,
                                    bold: true
                                }

                            },
                            defaultStyle: {
                                font: 'Times New Roman'
                            },

                            image: data,                            
                            width: 500,
                            
                        }]

                    };
                    pdfMake.createPdf(docDefinition).download("hello.pdf");
                //    pdfMake.createPdf(docDefinition).saveAs();



                }
            });
        }

    }

})();
