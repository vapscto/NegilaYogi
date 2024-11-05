

(function () {
    'use strict';
    angular
.module('app')
.controller('hhs01Controller', hhs01Controller)

    hhs01Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function hhs01Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.HHS_I_IV_grid = false;
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("HHSReport_PreNurs/Getdetails").
       then(function (promise) {
           
           $scope.yearlt = promise.yearlist;
           $scope.clslist = promise.classlist;
           $scope.seclist = promise.seclist;
           $scope.amstlt = promise.amstlist;
           $scope.studlist = promise.hhstudlist;
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
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "AMST_Id": $scope.amsT_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                $scope.fillterms = [];
                apiService.create("HHSReport_PreNurs/savedetails", data).
                         then(function (promise) {
                             
                          
                             $scope.personality_status = promise.personality_status;
                             $scope.co_curricular_activity = promise.co_curricular_activity;

                            
                                 $scope.HHS_I_IV_grid = true;
                                 $scope.stu_details = promise.stu_details;
                                 $scope.stuname = promise.stu_details[0].stuname;
                                 $scope.asmcL_ClassName = promise.stu_details[0].asmcL_ClassName;
                                 $scope.asmC_SectionName = promise.stu_details[0].asmC_SectionName;
                                 $scope.amaY_RollNo = promise.stu_details[0].amaY_RollNo;
                                 $scope.amsT_RegistrationNo = promise.stu_details[0].amsT_RegistrationNo;
                                 $scope.amsT_FatherName = promise.stu_details[0].amsT_FatherName;
                                 $scope.amsT_PerStreet = promise.stu_details[0].amsT_PerStreet;
                                 $scope.amsT_PerArea = promise.stu_details[0].amsT_PerArea;
                                 $scope.amsT_PerCity = promise.stu_details[0].amsT_PerCity;
                                 $scope.ivrmmS_Name = promise.stu_details[0].ivrmmS_Name;
                                 $scope.ivrmmC_CountryName = promise.stu_details[0].ivrmmC_CountryName;
                                 $scope.amsT_PerPincode = promise.stu_details[0].amsT_PerPincode;
                                 $scope.amsT_DOB = promise.stu_details[0].amsT_DOB;
                                 $scope.asmaY_Year = promise.stu_details[0].asmaY_Year;

                                 angular.forEach($scope.yearlt, function (y) {
                                     if (y.asmaY_Id == $scope.asmaY_Id) {
                                         $scope.yearname = y.asmaY_Year;
                                     }
                                 })

                                 angular.forEach($scope.clslist, function (c) {
                                     if (c.asmcL_Id == $scope.asmcL_Id) {
                                         $scope.clasname = c.asmcL_ClassName;
                                     }
                                 })

                                 angular.forEach($scope.seclist, function (s) {
                                     if (s.asmS_Id == $scope.asmS_Id) {
                                         $scope.sectioname = s.asmC_SectionName;
                                     }
                                 })

                                 $scope.fillterms = promise.fillterms;
                                 $scope.fillskills = promise.fillskills;
                                 $scope.fillskillarea = promise.fillskillarea;
                                 $scope.filltermmarks = promise.filltermmarks;
                                

                                 


                                 
                               
                                 var arrayText = [{ img: '/images/Pre-hutching/r11.png' }, { img: '/images/Pre-hutching/r12.png' }, { img: '/images/Pre-hutching/r13.png' }, { img: '/images/Pre-hutching/r1.png' }, { img: '/images/Pre-hutching/r2.png' }, { img: '/images/Pre-hutching/r3.png' }, { img: '/images/Pre-hutching/r4.png' }, { img: '/images/Pre-hutching/01.png' }, { img: '/images/Pre-hutching/02.png' }, { img: '/images/Pre-hutching/03.png' }];
                                 console.log(arrayText);

                                 $scope.imglist = [];
                                 
                                 $scope.imglist.push({ ll:arrayText })
                                 console.log($scope.subject_list);
                                 console.log($scope.imglist);

                                 //var a = $scope.imglist[0].ll[0].img;
                                 //alert(a)
                                 var ccc = 0
                                 angular.forEach($scope.fillskills, function (mm) {
                                    
                                     mm.img = $scope.imglist[0].ll[ccc].img;
                                     ccc += 1;

                                     if (ccc == 4) {
                                         
                                         mm.img1 = '/images/Pre-hutching/footer.png';
                                         mm.img2 = '/images/Pre-hutching/header1.png';
                                     }
                                     if (ccc == 8) {
                                         mm.img1 = '/images/Pre-hutching/footer1.png';
                                         
                                     }
                                 })

                                 
                                 console.log($scope.fillskills)


                                 var ccc = 1
                                 angular.forEach($scope.fillterms, function (mm) {
                                     if (ccc == $scope.fillterms.length -1) {
                                         mm.img = $scope.imglist[0].ll[ccc].img;
                                     }
                                    
                                     ccc += 1;
                                 })
                              
                             //MB
                                 angular.forEach($scope.fillskills, function (mm) {
                                     var temp_array = [];
                                     angular.forEach($scope.fillskillarea, function (mn) {
                                         if (mn.ecS_Id == mm.ecS_Id)
                                             temp_array.push(mn);
                                     })
                                     mm.areas = temp_array;
                                 })
                             //MB

                                 console.log($scope.subject_list);

                                 //---------------Personlity
                                 $scope.per_status = [];
                                 $scope.month_status = [];
                                 $scope.remarks_status = [];
                                 if ($scope.personality_status.length != 0) {
                                     angular.forEach($scope.personality_status, function (eps) {
                                         $scope.per_status.push({ eP_Id: eps.eP_Id, eP_PersonlaityName: eps.eP_PersonlaityName, epcR_Id: eps.epcR_Id });
                                         $scope.month_status.push({ month_Id: eps.month_Id, ivrM_Month_Name: eps.ivrM_Month_Name });
                                         //angular.forEach($scope.month_status, function (mnt) {
                                         //    if (eps.month_Id == mnt.month_Id) {
                                         //        angular.forEach($scope.per_status, function (per_s) {
                                         //            if (per_s.epcR_Id == eps.epcR_Id) {

                                         //                per_s.month_Id = mnt.month_Id;
                                         //                per_s.rekstus = eps.epcR_RemarksName;
                                         //               // $scope.remarks_status.push({ epcR_Id: eps.epcR_Id, epcR_RemarksName: eps.epcR_RemarksName });
                                         //            }
                                         //        })
                                         //    }

                                         //})

                                     })
                                 }
                                 $scope.co_activity = [];
                                 $scope.month_activity = [];
                                 $scope.remarks_activity = [];
                                 if ($scope.co_curricular_activity.length != 0) {
                                     angular.forEach($scope.co_curricular_activity, function (cca) {
                                         $scope.co_activity.push({ ecC_Id: cca.ecC_Id, ecC_CoCurricularName: cca.ecC_CoCurricularName });
                                         $scope.month_activity.push({ month_Id: cca.month_Id, ivrM_Month_Name: cca.ivrM_Month_Name });
                                     })
                                 }
                            


                         })
            };

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
            $scope.print_HHS02 = function () {
                var innerContents = document.getElementById("HHS02").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                   '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/ProgressReport/HHS01/HHS01Pdf.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            
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
        $scope.ProgressReport = function () {
            var innerContents = document.getElementById("ProgressReport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/ProgressReport/HHS01/HHS01Pdf.css" />' +
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
        }
        $scope.OnAcdyear = function (asmaY_Id) {

            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';

            $scope.amstid = '';
            $scope.fillstudents = [];
            $scope.section = [];
            $scope.classarray = [];
            apiService.getURI("HHSReport_PreNurs/getclass", asmaY_Id).
          then(function (promise) {
              $scope.classarray = promise.fillclass;
              // console.log($scope.classarray);
          })


        }



        $scope.OnClass = function (asmcL_Id) {

            $scope.asmS_Id = '';

            $scope.amstid = '';
            $scope.asmcL_Id = asmcL_Id;
            // alert(asmaY_Id)
            $scope.section = [];
            $scope.fillstudents = [];
            var data = {
                "ASMCL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HHSReport_PreNurs/Getsection", data).
          then(function (promise) {

              //  
              $scope.section = promise.fillsection;



          })


        }
        $scope.OnSection = function (asmS_Id) {


            $scope.amstid = '';
            $scope.asmS_Id = asmS_Id;

            $scope.fillstudents = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": asmS_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HHSReport_PreNurs/GetAttendence", data).
          then(function (promise) {

              //

              // $scope.indattendance = true;
              $scope.fillstudents = promise.fillstudents;





          })


        }







    }

})();