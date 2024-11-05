(function () {
    'use strict';
    angular
.module('app')
        .controller('StudentAchievementReportController', StudentAchievementReportController)

    StudentAchievementReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function StudentAchievementReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.pagesrecord = {};
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;

        $scope.table_flag = false;
        $scope.report_flag = false;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        //$scope.itemsPerPage = 10;



        $scope.printdatatable = [];
        $scope.sortBy = function (propertyName) {
            
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.exportToExcel = function (tableId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select Records to be Exported");
            }

        }
        $scope.toggleAll = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (obj.regno).indexOf($scope.searchValue) >= 0 || (obj.admno).indexOf($scope.searchValue) >= 0 || (obj.stuFN).indexOf($scope.searchValue) >= 0 || (obj.achivement).indexOf($scope.searchValue) >= 0;
        }



        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all2 = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("StudentAchievementReport/getdata", pageid).
        then(function (promise) {
            $scope.yearlist = promise.allAcademicYear;
            $scope.classlist = promise.allClass;
            $scope.sectionlist = promise.allSection;
            //  $scope.studlist = promise.adm_m_student;
            //$scope.achivementlist = promise.allAchivement;
        })
        }


        $scope.report123 = false;
        $scope.excel_flag = true;
        $scope.submitted = false;




        //$scope.Report = function (optradio) {
        $scope.Report = function () {
            $scope.angularData =
         {
             'nameList': []
         };

            $scope.vals = [];
            
            $scope.students = [];
            $scope.printdatatable = [];
            $scope.searchValue = "";
            //$scope: ASMAY_Id = $scope.asmaY_Id,


            var acedamicId = $scope.asmaY_Id;
            var sectionId = $scope.asmC_Id;
            var classId = $scope.asmcL_Id;
            var studId = $scope.amsT_Id;



            if (acedamicId == undefined || acedamicId == "" || acedamicId == null) {
                acedamicId = 0;
            }
            if (classId == undefined || classId == "" || classId == null) {
                classId = 0;
            }
            if (sectionId == undefined || sectionId == "" || sectionId == null) {
                sectionId = 0;
            }
            if (studId == undefined || studId == "" || studId == null) {
                studId = 0;
            }

            //if (AchiveId == undefined || AchiveId == "" || AchiveId == null)
            //{
            //    AchiveId = 0;
            //}






            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "AMST_Id": studId,
                    // "AMSTEC_Id": AchiveId
                }


                apiService.create("StudentAchievementReport/Studdetails", data)
              .then(function (promise) {

                  if (promise.studentlist == null || promise.studentlist.length == 0) {
                      swal("Record Not Found !");
                      $scope.report123 = false;
                      $scope.excel_flag = true;
                  }
                  else {
                      $scope.students = promise.studentlist;
                      $scope.presentCountgrid = $scope.students.length;
                      for (var i = 0; i < promise.studentlist.length; i++) {
                          var name = promise.studentlist[i].stuFN;
                          if (promise.studentlist[i].stuMN !== null) {
                              name += " " + promise.studentlist[i].stuMN;
                          }
                          if (promise.studentlist[i].stuLN != null) {
                              name += " " + promise.studentlist[i].stuLN;
                          }
                          $scope.vals.push(name);
                      }
                      angular.forEach($scope.vals, function (v, k) {
                          $scope.angularData.nameList.push({
                              'fullname': v
                          });
                      });
                      var j = 0;
                      angular.forEach($scope.students, function (obj) {
                          //Using bracket notation
                          obj["fullname"] = $scope.angularData.nameList[j].fullname;
                          j++;
                      });

                      angular.forEach($scope.students, function (objectt) {
                          if (objectt.fullname.length > 0) {
                              var string = objectt.fullname
                              objectt.namme = string.replace(/  +/g, ' ');
                          }
                      })
                      $scope.report123 = true;
                      $scope.excel_flag = false;
                      console.log(promise.studentlist);
                  }
              }
                );
            }
            else {
                $scope.submitted = true;
            }
        }

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });


        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
            $scope.excel_flag = true;
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.amsTEC_Id = "";
            $scope.amsT_Id = "";
            $scope.report123 = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }


        $scope.All_Individual = function () {
            $scope.angularData =
          {
              'nameList': []
          };

            $scope.vals = [];

            
            if ($scope.Admnoallind == "All") {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.amsT_Id = "";
                $scope.amsTEC_Id = "";
                $scope.report_flag = false;
                $scope.stud_name = false;
                $scope.ach_name = false;
                $scope.submitted = false;
                $scope.report123 = false;

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }

            if ($scope.Admnoallind == "Indi") {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.amsT_Id = "";
                $scope.amsTEC_Id = "";
                $scope.stud_name = true;
                $scope.ach_name = true;
                $scope.report_flag = false;
                $scope.submitted = false;
                $scope.report123 = false;

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        }




        $scope.stdnamesyear = function () {
            
            if ($scope.Admnoallind == "Indi") {

                var acedamicId = $scope.asmaY_Id;
                var sectionId = $scope.asmC_Id;
                var classId = $scope.asmcL_Id;
                //var studId = $scope.amsT_Id;
                //var AchiveId = $scope.amsTEC_Id;
                
                if (acedamicId == undefined || acedamicId == "" || acedamicId == null) {
                    acedamicId = 0;
                }
                if (classId == undefined || classId == "" || classId == null) {
                    classId = 0;
                }
                if (sectionId == undefined || sectionId == "" || sectionId == null) {
                    sectionId = 0;
                }
                //if (studId == undefined || studId == "" || studId == null)
                //{
                //    studId = 0;
                //}

                //if (AchiveId == undefined || AchiveId == "" || AchiveId == null) {
                //    AchiveId = 0;
                //}


                //var flag = 'S';


                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "asms_id": sectionId,
                    //"flag123": flag,
                    //"AMST_Id": studId,

                }
                
                apiService.create("StudentAchievementReport/stdnamesyear", data).
         then(function (promise) {
             if (promise.studentlist123 != null && promise.studentlist123.length > 0) {
                 $scope.studlist = promise.studentlist123;
                 $scope.report_flag = false;
             }
             else {
                 swal("Student is Not Available For This Selection");
                 $scope.report_flag = true;
             }

         })
            };
        }





        $scope.stdachment = function () {
            $scope.angularData =
          {
              'nameList': []
          };

            $scope.vals = [];

            var acedamicId = $scope.asmaY_Id;
            var sectionId = $scope.asmC_Id;
            var classId = $scope.asmcL_Id;
            var studId = $scope.amsT_Id;
            // var AchiveId = $scope.amsTEC_Id;
            
            if (acedamicId == undefined || acedamicId == "" || acedamicId == null) {
                acedamicId = 0;
            }
            if (classId == undefined || classId == "" || classId == null) {
                classId = 0;
            }
            if (sectionId == undefined || sectionId == "" || sectionId == null) {
                sectionId = 0;
            }
            if (studId == undefined || studId == "" || studId == null) {
                studId = 0;
            }

            //if (AchiveId == undefined || AchiveId == "" || AchiveId == null) {
            //    AchiveId = 0;
            //}

            var flag = 'L';
            var data = {
                "ASMAY_Id": acedamicId,
                "ASMCL_Id": classId,
                "asms_id": sectionId,
                "flag123": flag,
                "AMST_Id": studId,

            }
            apiService.create("StudentAchievementReport/stdnamesyear", data).
     then(function (promise) {

         if (promise.studentlist123 == null || promise.studentlist123.length == 0) {
             // swal('Records not found..!');

             $scope.achivementlist = [];
         }
         else {
             $scope.achivementlist = promise.studentlist123;
         }

     })
        };
    }
})();

