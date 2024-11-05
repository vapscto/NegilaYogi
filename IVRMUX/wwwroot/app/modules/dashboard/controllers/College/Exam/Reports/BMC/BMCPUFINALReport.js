
//(function () {
//    'use strict';
//    angular
//.module('app')
//.controller('BLPUFINALReportController', BLPUFINALReportController)

//    BLPUFINALReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
//    function BLPUFINALReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


//        $scope.DeleteRecord = {};
//        $scope.EditRecord = {};
//        $scope.obj = {};
//        $scope.studentlist = false;
//        $scope.currentPage = 1;
//        $scope.itemsPerPage = 5;
//        $scope.passyear = '';
//        $scope.pass = '';
//        //TO  GEt The Values iN Grid

//        $scope.BindData = function () {
//            apiService.getDATA("BaldwinPUReport/Getdetails").
//       then(function (promise) {
           
//           $scope.yearlt = promise.yearlist;
//           $scope.clslist = promise.classlist;
//           $scope.seclist = promise.seclist;
//           $scope.amstlt = promise.amstlist;
//           $scope.exsplt = promise.exmstdlist;
//           //  $scope.gridOptions.data = promise.studmaplist;
//       })
//        };

      

//        $scope.sort = function (keyname) {
//            $scope.sortKey = keyname;   //set the sortKey to the param passed
//            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
//        }

//        $scope.interacted = function (field) {
//            return $scope.submitted;
//        };



//        // TO Save The Data
//        $scope.submitted = false;
//        $scope.saveddata = function () {
            
//            angular.forEach($scope.yearlt, function (yr) {
//                if (yr.asmaY_Id == $scope.asmaY_Id) {
//                    $scope.year = yr.asmaY_Year;
//                }
//            })
//            $scope.submitted = true;
//            if ($scope.myForm.$valid) {

//                var data = {
//                    "EME_Id": $scope.emE_Id,
//                    "ASMAY_Id": $scope.asmaY_Id,
//                    "ASMCL_Id": $scope.asmcL_Id,
//                    "ASMS_Id": $scope.asmS_Id
//                }
//                var config = {
//                    headers: {
//                        'Content-Type': 'application/json;'
//                    }
//                }
//                apiService.create("BaldwinPUReport/savedetails", data).
//                         then(function (promise) {
                             
//                             if (promise.savelist.length > 0) {
//                                 //inst name binding

//                                 $scope.pass = $scope.passyear;
//                                 $scope.instname = promise.instname;
//                                 $scope.inst_name = $scope.instname[0].mI_Name;

//                                 var temp_list = [];
//                                 for (var x = 0; x < promise.savelist.length; x++) {
//                                     var stu_id = promise.savelist[x].amsT_Id;
//                                     var stu_subj_list = [];
//                                     angular.forEach(promise.savelist, function (opq) {
//                                         if (opq.amsT_Id == stu_id) {
//                                             stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest, word: convertNumberToWords(opq.estmpS_ObtainedMarks) });
//                                         }
//                                     })
//                                     if (temp_list.length == 0) {

//                                         temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark,  sub_list: stu_subj_list });
//                                     }
//                                     else if (temp_list.length > 0) {
//                                         var already_cnt = 0;
//                                         angular.forEach(temp_list, function (opq1) {

//                                             if (opq1.student_id == stu_id) {
//                                                 already_cnt += 1;

//                                             }
//                                         })
//                                         if (already_cnt == 0) {
//                                             temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark, sub_list: stu_subj_list });
//                                         }
//                                     }

//                                 }
//                                 $scope.exam = promise.savelist[0].emE_ExamName;
//                                 $scope.exm_sublist = promise.subjlist;
//                                 $scope.processtot = promise.savelisttot;
//                                 $scope.subj_grade_remarks = promise.grade_details;
//                                 if (promise.clstchname.length > 0) {
//                                     $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
//                                 } else {
//                                     $scope.clastechname = "";
//                                 }
//                                 //  $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;


//                                 $scope.report_list = temp_list;
//                                 $scope.stud_work_attendence = promise.work_attendence;
//                                 $scope.stud_present_attendence = promise.present_attendence;

//                                 $scope.tempss = [];
//                                 var ss = 0;
//                                 angular.forEach($scope.processtot, function (bb) {
//                                     bb.word = convertNumberToWords(bb.estmP_TotalObtMarks)
//                                 })

//                                 console.log($scope.report_list);
//                                 console.log($scope.exm_sublist);
//                                 var cn = 0;
//                                 angular.forEach($scope.exm_sublist, function (ff) {
//                                     cn += 1;
//                                     if (cn==1) {
//                                         ff.a = 1;

//                                     }
//                                 })

//                             } else {
//                                 swal('No record Found');
//                             }
//                         })
//            }
//        };

//        function convertNumberToWords(amount) {
            
//            if (amount==0) {
//                words_string = 'ZERO';
//            } else {

//                var words = new Array();
//                words[0] = '';
//                words[1] = 'One';
//                words[2] = 'Two';
//                words[3] = 'Three';
//                words[4] = 'Four';
//                words[5] = 'Five';
//                words[6] = 'Six';
//                words[7] = 'Seven';
//                words[8] = 'Eight';
//                words[9] = 'Nine';
//                words[10] = 'Ten';
//                words[11] = 'Eleven';
//                words[12] = 'Twelve';
//                words[13] = 'Thirteen';
//                words[14] = 'Fourteen';
//                words[15] = 'Fifteen';
//                words[16] = 'Sixteen';
//                words[17] = 'Seventeen';
//                words[18] = 'Eighteen';
//                words[19] = 'Nineteen';
//                words[20] = 'Twenty';
//                words[30] = 'Thirty';
//                words[40] = 'Forty';
//                words[50] = 'Fifty';
//                words[60] = 'Sixty';
//                words[70] = 'Seventy';
//                words[80] = 'Eighty';
//                words[90] = 'Ninety';
//                amount = amount.toString();
//                var atemp = amount.split(".");
//                var number = atemp[0].split(",").join("");
//                var n_length = number.length;
//                var words_string = "";
//                if (n_length <= 9) {
//                    var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
//                    var received_n_array = new Array();
//                    for (var i = 0; i < n_length; i++) {
//                        received_n_array[i] = number.substr(i, 1);
//                    }
//                    for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
//                        n_array[i] = received_n_array[j];
//                    }
//                    for (var i = 0, j = 1; i < 9; i++, j++) {
//                        if (i == 0 || i == 2 || i == 4 || i == 7) {
//                            if (n_array[i] == 1) {
//                                n_array[j] = 10 + parseInt(n_array[j]);
//                                n_array[i] = 0;
//                            }
//                        }
//                    }
//                    var value = "";
//                    for (var i = 0; i < 9; i++) {
//                        if (i == 0 || i == 2 || i == 4 || i == 7) {
//                            value = n_array[i] * 10;
//                        } else {
//                            value = n_array[i];
//                        }
//                        if (value != 0) {
//                            words_string += words[value] + " ";
//                        }
//                        if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
//                            words_string += "Crores ";
//                        }
//                        if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
//                            words_string += "Lakhs ";
//                        }
//                        if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
//                            words_string += "Thousand ";
//                        }
//                        if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
//                            words_string += "Hundred and ";
//                        } else if (i == 6 && value != 0) {
//                            words_string += "Hundred ";
//                        }
//                    }
//                    words_string = words_string.split("  ").join(" ");
//                }
                    
//            }
           
//            return words_string;
//        }

//        $scope.cancel = function () {
//            $scope.asmcL_Id = ""
//            $scope.emcA_Id = ""
//            $scope.asmaY_Id = ""
//            $scope.emG_Id = ""
//            $scope.asmS_Id = ""
//            $scope.subjectlt = ""
//            $scope.subjectlt1 = ""
//            $scope.studentlist = false;
//            $state.reload();
//        }

//        $scope.toggleAll = function () {
            
//            var toggleStatus = $scope.all;
//            angular.forEach($scope.subjectlt1, function (itm) {
//                itm.xyz = toggleStatus;

//            });
//        }

//        $scope.optionToggled = function (chk_box) {
//            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
//        }


//        //to print
//        $scope.printToCart = function () {
//            var innerContents = document.getElementById("Baldwin").innerHTML;
//            var popupWinindow = window.open('');
//            popupWinindow.document.open();
//            popupWinindow.document.write('<html><head>' +
//                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
//                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBPUProgressReportPdf.css" />' +
//              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
//            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
//            popupWinindow.document.close();
//        }

//        $scope.get_totalmin = function (exm_subjs, stu_subjs) {
            

//            $scope.stu_grandmin_marks = 0;
//            angular.forEach(exm_subjs, function (itm) {
//                if (itm.eyceS_AplResultFlg) {
//                    angular.forEach(stu_subjs, function (itm1) {
//                        if (itm1.ismS_Id == itm.ismS_Id) {
//                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
//                        }
//                    })
//                }

//            })
//        }
//    }
//})();



(function () {
    'use strict';
    angular
        .module('app')
        .controller('BLPUFINALReportController', BLPUFINALReportController)

    BLPUFINALReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BLPUFINALReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.passyear = '';
        $scope.pass = '';
        $scope.report = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("BaldwinPUReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                    //  $scope.gridOptions.data = promise.studmaplist;
                })
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            angular.forEach($scope.yearlt, function (yr) {
                if (yr.asmaY_Id == $scope.asmaY_Id) {
                    $scope.year = yr.asmaY_Year;
                }
            })
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BaldwinPUReport/savedetails", data).
                    then(function (promise) {

                        if (promise.savelist.length > 0) {
                            //inst name binding
                            $scope.report = true;
                            $scope.pass = $scope.passyear;
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;

                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, ESTMPS_ClassAverage: opq.estmpS_ClassAverage, ESTMPS_SectionAverage: opq.estmpS_SectionAverage, ESTMPS_ClassHighest: opq.estmpS_ClassHighest, ESTMPS_SectionHighest: opq.estmpS_SectionHighest, word: convertNumberToWords(opq.estmpS_ObtainedMarks) });
                                    }
                                })
                                if (temp_list.length == 0) {

                                    temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark, sub_list: stu_subj_list });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {

                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;

                                        }
                                    })
                                    if (already_cnt == 0) {
                                        temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_TotalGradeRemark: promise.savelist[x].estmP_TotalGradeRemark, sub_list: stu_subj_list });
                                    }
                                }

                            }
                            $scope.exam = promise.savelist[0].emE_ExamName;
                            $scope.exm_sublist = promise.subjlist;
                            $scope.processtot = promise.savelisttot;
                            $scope.subj_grade_remarks = promise.grade_details;
                            if (promise.clstchname.length > 0) {
                                $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                            } else {
                                $scope.clastechname = "";
                            }
                            //  $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;


                            $scope.report_list = temp_list;
                            $scope.stud_work_attendence = promise.work_attendence;
                            $scope.stud_present_attendence = promise.present_attendence;

                            $scope.tempss = [];
                            var ss = 0;
                            angular.forEach($scope.processtot, function (bb) {
                                bb.word = convertNumberToWords(bb.estmP_TotalObtMarks)
                            })



                            console.log($scope.report_list);
                            console.log($scope.exm_sublist);
                            var cn = 0;

                            var xx = $scope.exm_sublist.length / 2;
                            $scope.temparry = [];
                            $scope.temparry.push({ part: 'PART-I LANGUAGES' })
                            $scope.temparry.push({ part: 'PART-II OPTIONALS' })
                            var a1 = [];
                            var a2 = [];
                            angular.forEach($scope.exm_sublist, function (ff) {
                                cn += 1;
                                if (ff.ismS_SubjectName != '') {
                                    if (ff.ismS_SubjectName.toLowerCase() == 'kannada' || ff.ismS_SubjectName.toLowerCase() == 'english' || ff.ismS_SubjectName.toLowerCase() == 'french' || ff.ismS_SubjectName.toLowerCase() == 'hindi' || ff.ismS_SubjectName.toLowerCase() == 'sanskrit' || ff.ismS_SubjectName.toLowerCase() == 'urdu') {
                                        ff.a = 1;
                                        a1.push(ff)
                                    }
                                    else {
                                        a2.push(ff)
                                    }
                                }

                            })
                            var l = 0;
                            angular.forEach($scope.temparry, function (kk) {
                                if (l == 0) {
                                    l += 1;
                                    kk.sb = a1;
                                    kk.gg = a1.length;
                                }
                                else {
                                    kk.sb = a2;
                                    kk.oo = a2.length;
                                }


                            })

                            console.log($scope.temparry)

                        } else {
                            swal('No record Found');
                        }
                    })
            }
        };

        function convertNumberToWords(amount) {

            if (amount == 0) {
                words_string = 'ZERO';
            } else {

                var words = new Array();
                words[0] = '';
                words[1] = 'One';
                words[2] = 'Two';
                words[3] = 'Three';
                words[4] = 'Four';
                words[5] = 'Five';
                words[6] = 'Six';
                words[7] = 'Seven';
                words[8] = 'Eight';
                words[9] = 'Nine';
                words[10] = 'Ten';
                words[11] = 'Eleven';
                words[12] = 'Twelve';
                words[13] = 'Thirteen';
                words[14] = 'Fourteen';
                words[15] = 'Fifteen';
                words[16] = 'Sixteen';
                words[17] = 'Seventeen';
                words[18] = 'Eighteen';
                words[19] = 'Nineteen';
                words[20] = 'Twenty';
                words[30] = 'Thirty';
                words[40] = 'Forty';
                words[50] = 'Fifty';
                words[60] = 'Sixty';
                words[70] = 'Seventy';
                words[80] = 'Eighty';
                words[90] = 'Ninety';
                amount = amount.toString();
                var atemp = amount.split(".");
                var number = atemp[0].split(",").join("");
                var n_length = number.length;
                var words_string = "";
                if (n_length <= 9) {
                    var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                    var received_n_array = new Array();
                    for (var i = 0; i < n_length; i++) {
                        received_n_array[i] = number.substr(i, 1);
                    }
                    for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                        n_array[i] = received_n_array[j];
                    }
                    for (var i = 0, j = 1; i < 9; i++ , j++) {
                        if (i == 0 || i == 2 || i == 4 || i == 7) {
                            if (n_array[i] == 1) {
                                n_array[j] = 10 + parseInt(n_array[j]);
                                n_array[i] = 0;
                            }
                        }
                    }
                    var value = "";
                    for (var i = 0; i < 9; i++) {
                        if (i == 0 || i == 2 || i == 4 || i == 7) {
                            value = n_array[i] * 10;
                        } else {
                            value = n_array[i];
                        }
                        if (value != 0) {
                            words_string += words[value] + " ";
                        }
                        if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Crores ";
                        }
                        if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Lakhs ";
                        }
                        if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Thousand ";
                        }
                        if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                            words_string += "Hundred and ";
                        } else if (i == 6 && value != 0) {
                            words_string += "Hundred ";
                        }
                    }
                    words_string = words_string.split("  ").join(" ");
                }

            }

            return words_string;
        }

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;

            });
        }

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }


        //to print
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBPUProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {


            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (itm1.ismS_Id == itm.ismS_Id) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    })
                }

            })
        };

        $scope.OnAcdyear = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("CumulativeReport/onchangeyear", data).then(function (promise) {

                if (promise != null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length == 0) {
                        swal("No Class Is Mapped For Selected Academic Year");
                    }
                } else {
                    swal("No Class Is Mapped For Selected Academic Year");
                }

            });
        };

        $scope.onchangeclass = function () {

            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("CumulativeReport/onchangeclass", data).then(function (promise) {

                if (promise != null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length == 0) {
                        swal("No Section Is Mapped For Selected Class");
                    }

                } else {
                    swal("No Section Is Mapped For Selected Class");
                }

            });
        };

        $scope.onchangesection = function () {

            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("CumulativeReport/onchangesection", data).then(function (promise) {

                if (promise != null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length == 0) {
                        swal("No Exam Is Mapped For Selected Details ");
                    }
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                }

            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
        };


    }

})();