(function () {
    'use strict';
    angular
.module('app')
.controller('BatchwiseStudentMappingController', BatchwiseStudentMappingController)

    BatchwiseStudentMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function BatchwiseStudentMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));







        $scope.adtableBwsl = false;
        $scope.Studlist = false;
        $scope.EditdStudlist = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.IsHiddenmiddle = false;
        $scope.currentPage_up = 1;
        // $scope.itemsPerPage_up = 5;
        $scope.currentPage_down = 1;
        $scope.itemsPerPage_down = paginationformasters;
        $scope.page_up = "page_up";
        $scope.page_down = "page_down";
        //$scope.currentPage3 = 1;
        //$scope.itemsPerPage3 = 5;
        //$scope.currentPage3 = 1;
        $scope.paginate1 = "paginate1";
        $scope.studentList = [];

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }



        $scope.ShowHidedown = function () {

            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.ShowHidemiddle = function () {


            $scope.IsHiddenmiddle = $scope.IsHiddenmiddle ? false : true;
        }


        $scope.sortBy_up = function (propertyName_up) {
            $scope.reverse_up = ($scope.propertyName_up === propertyName_up) ? !$scope.reverse_up : false;
            $scope.propertyName_up = propertyName_up;
        };
        $scope.sortBy_down = function (propertyName_down) {
            $scope.sortKey = propertyName_down;
            $scope.reverse_down = !$scope.reverse_down;
        };




        $scope.angularData =
                  {
                      'nameList': []
                  };

        $scope.vals = [];

        // On load page get all dropdown list
        $scope.LoadData = function () {          
            apiService.getURI("BatchwiseStudentMapping/GetstudentdetailsbyYearandInstitute1",2).
        then(function (promise) {
            
            $scope.yearList = promise.yearList;
            $scope.classList = promise.classList;
            $scope.sectionList = promise.sectionList;
            $scope.subjectList = promise.subjectList;
            $scope.batchList = promise.batchList;
            if (promise.batchcount > 0) {
                $scope.subjectBatchList = promise.subjectBatchList;
                $scope.presentCountgrid = $scope.subjectBatchList.length;
                for (var i = 0; i < promise.subjectBatchList.length; i++) {

                    var name = promise.subjectBatchList[i].amsT_FirstName;

                    if (promise.subjectBatchList[i].amsT_MiddleName !== null) {
                        name += " " + promise.subjectBatchList[i].amsT_MiddleName;
                    }
                    if (promise.subjectBatchList[i].amsT_LastName != null) {
                        name += " " + promise.subjectBatchList[i].amsT_LastName;
                    }
                    $scope.vals.push(name);
                }
                angular.forEach($scope.vals, function (v, k) {
                    $scope.angularData.nameList.push({
                        'fullname': v
                    });
                });
                var j = 0;
                angular.forEach($scope.subjectBatchList, function (obj) {
                    //Using bracket notation
                    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                    j++;
                });
                angular.forEach($scope.subjectBatchList, function (objectt) {
                    if (objectt.fullname.length > 0) {
                        var string = objectt.fullname
                        objectt.namme = string.replace(/  +/g, ' ');
                    }
                })


                $scope.adtableBwsl = true;
            }
            else {
                $scope.adtableBwsl = false;
                // swal("No Records Found.....!!");
            }

            //$scope.batchList = promise.batchList;
        })
        }




        // checkbox
        $scope.chckedIndexs = [];


        $scope.checkAll = function () {
            if ($scope.selectedAll) {

                $scope.selectedAll = true;
            } else {
                $scope.selectedAll = false;
            }
            angular.forEach($scope.studentList, function (student) {
                student.Selected = $scope.selectedAll;

                if ($scope.chckedIndexs.indexOf(student) === -1) {
                    if (student.Selected) $scope.chckedIndexs.push(student);
                }
            });
        }

        $scope.test = function (data) {
            if ($scope.selectedAll = true) {
                $scope.selectedAll = false
            }
            //console.log(data.Selected);
            if ($scope.chckedIndexs.indexOf(data) === -1) {
                if (data.Selected) $scope.chckedIndexs.push(data);
            }
            else {
                $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(data), 1);
            }
            $scope.selectedAll = $scope.studentList.every(function (options) {
                return options.Selected;
            })
        }



        $scope.submitted = false;
        $scope.saveDetails = function (user) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.editrecord == true) {
                    var plg = $scope.editedStudentList;
                }
                else {
                    if ($scope.chckedIndexs.length > 0) {
                         plg = $scope.chckedIndexs;

                    }
                    else {
                        swal('Select Atleast One Checkbox to Proceed');
                        return;
                    }
                }
                
                var data = {
                    "ASASB_Id": $scope.ASASB_Id,
                    "ASMAY_Id": $scope.AMAY_Id,
                    "ASMCL_Id": $scope.AMCL_Id,
                    "ASMS_Id": $scope.AMS_Id,
                    "AMSU_Id": $scope.ISMS_Id,
                    "ASASB_BatchName": $scope.ASASB_BatchName,
                    "ASASB_Id1": $scope.ASASB_Id1,
                    "type12": $scope.type12,
                    selectedstudents: plg
                }
                
                apiService.create("BatchwiseStudentMapping/", data).
                  then(function (promise) {
                      $scope.chckedIndexs = [];
                      //if (promise.message != "" && promise.message != null) {
                      //    alert(promise.message);
                      //    // $state.reload();
                      //    //  return;
                      //}
                      if (promise.returnVal == true) {
                          if (promise.message != "" && promise.message != null) {
                              swal('Record Saved/Updated Successfully', promise.message);
                          }
                          else {
                              swal('Record Saved/Updated Successfully');
                          }

                          //$state.reload();
                      }
                      else if (promise.returnVal == false) {
                          if (promise.message != null && promise.message != "") {
                              swal('Record not Saved/Updated', promise.message);
                          }
                          else {
                              swal('Record not Saved/Updated');
                          }
                         
                          // swal(promise.message);
                      }
                      else {
                          swal('Record not Saved/Updated');
                      }
                      $state.reload();
                  })
            }
        }

        //-- Clearid

        $scope.Clearid = function () {

            $state.reload();

        }



        // On load page get all dropdown list
        $scope.GetStudentListByYearAndCLassSection = function () {
            var data = {
                "FormType": "onselect",
                "ASMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.AMCL_Id,
                "ASMS_Id": $scope.AMS_Id

                //"ISMS_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("BatchwiseStudentMapping/GetstudentdetailsbyYearandInstitute", data).
        then(function (promise) {
            

            $scope.IsHiddenmiddle = true;
            $scope.Studlist = true;
            if (promise.studentCount > 0) {
                $scope.Studlist = true;
                $scope.studentList = promise.studentList;

                //for (var i = 0; i < promise.studentList.length; i++) {

                //    var name = promise.studentList[i].amsT_FirstName;

                //    if (promise.studentList[i].amsT_MiddleName !== null) {

                //        name += " " + promise.studentList[i].amsT_MiddleName;
                //    }

                //    if (promise.studentList[i].amsT_LastName != null) {
                //        name += " " + promise.studentList[i].amsT_LastName;
                //    }

                //    $scope.vals.push(name);
                //}

                //angular.forEach($scope.vals, function (v, k) {
                //    $scope.angularData.nameList.push({
                //        'fullname': v
                //    });
                //});

                //var j = 0;
                //angular.forEach($scope.studentList, function (obj) {
                //    //Using bracket notation
                //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                //    j++;
                //});
                //angular.forEach($scope.studentList, function (objectt) {
                //    if (objectt.fullname.length > 0) {
                //        var string = objectt.fullname
                //        objectt.namme_chang = string.replace(/  +/g, ' ');
                //    }
                //})

                $scope.edit_button_flag = false;

            }
            else {
                $scope.Studlist = false;
                $scope.Studlist = false;

                swal('No Students are found to display');
                $scope.AMS_Id = "";
                $scope.ISMS_Id = "";
                $scope.ASASB_BatchName = "";
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        })
        }


        // filter student list
        $scope.filterStudentListByYearAndCLassSectionSubject = function () {
            var data = {
                "FormType": "filter",
                "ASMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.AMCL_Id,
                "ASMS_Id": $scope.AMS_Id,
                "AMSU_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("BatchwiseStudentMapping/GetstudentdetailsbyYearandInstitute", data).
        then(function (promise) {

            $scope.IsHiddenmiddle = true;

            if (promise.studentList.length > 0) {
                $scope.Studlist = true;
                $scope.adtableBwsl = true;
                $scope.studentList = promise.studentList;

            }
            else {
                $scope.adtableBwsl = false;
                $scope.Studlist = false;
                swal('No record found to display');
                $scope.ISMS_Id = "";
                $scope.ASASB_BatchName = "";
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        })
        }


        //edit record
        $scope.edit = function (data) {
            
            $scope.editedStudentList = [];
            $scope.EditdStudlist = false;
            $scope.Studlist = false;
            $scope.edit_button_flag = true;
            $scope.editrecord = true;
            $scope.ASASB_Id = data.asasB_Id;
            $scope.AMAY_Id = data.asmaY_Id;
            $scope.AMCL_Id = data.asmcL_Id;
            $scope.AMS_Id = data.asmS_Id;
            $scope.ISMS_Id = data.amsU_Id;
            $scope.ASASB_BatchName = data.asasB_BatchName;
            // $scope.GetStudentListByYearAndCLassSection();
            // $scope.EditStudentListByStudentId(data.amsT_Id);
            $scope.editedStudentList.push(data);
            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 5;
            $scope.currentPage3 = 1;
        }


        //delete record
        $scope.delete = function (id, SweetAlert) {
            var data = {
                "FormType": "deleterecord",
                "ASASBS_Id": id
            }

            // alert(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete the record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("BatchwiseStudentMapping/GetstudentdetailsbyYearandInstitute", data).
                    then(function (promise) {
                        if (promise.returnVal == true) {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Failed to Delete Record');
                        }

                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }

        $scope.interacted = function (field) {
            // alert(field);
            return $scope.submitted;
        };
        //$scope.isOptionsRequired = function () {
        //    
        //    return !$scope.studentList.some(function (options) {
        //        return options.Selected;
        //    });
        //}

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
            (angular.lowercase(obj.amsU_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.asasB_BatchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 (JSON.stringify(obj.asasB_StudentStrenth)).indexOf($scope.searchValue) >= 0
        }


        //on radio button selection
        $scope.getDataByType = function () {
            if ($scope.type12 == '1') {
                $scope.AMAY_Id = "";
                $scope.AMCL_Id = "";
                $scope.AMS_Id = "";
                $scope.ISMS_Id = "";
                $scope.ASASB_BatchName = "";
                $scope.Studlist = false;
            } else {
                $scope.AMAY_Id = "";
                $scope.AMCL_Id = "";
                $scope.AMS_Id = "";
                $scope.ISMS_Id = "";
                $scope.ASASB_Id1 = "";
                $scope.Studlist = false;
            }
        }

        //get the batchwise student list
        $scope.getbatchwisestdlist = function () {
            var data = {
                "ASMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.AMCL_Id,
                "ASMS_Id": $scope.AMS_Id,
                "AMSU_Id": $scope.ISMS_Id,
                "ASASB_Id": $scope.ASASB_Id1
            }
            apiService.create("BatchwiseStudentMapping/getbatchwisestdlist", data).
       then(function (promise) {
           if (promise.countbatchlist > 0) {
               $scope.countbatchlistarry = promise.batchwisestdlist;
               //for (var i = 0; i < $scope.countbatchlistarry.length; i++) {

               //}
               angular.forEach($scope.countbatchlistarry, function (stdbatchwise) {
                   angular.forEach($scope.studentList, function (stdsectionwise) {
                       if (stdsectionwise.amsT_AdmNo == stdbatchwise.admno && stdsectionwise.amsT_RegistrationNo == stdbatchwise.regno) {
                           stdsectionwise.Selected = true;
                           $scope.test(stdsectionwise);
                       } 
                   })
               })
           }
           else {
               swal("No Records Found");
               $state.reload();
           }
       });
        }


        //getting the batch name 
        $scope.getbatchname = function () {
            if ($scope.type12 == '2') {
                var data = {
                    "ASMAY_Id": $scope.AMAY_Id,
                    "ASMCL_Id": $scope.AMCL_Id,
                    "ASMS_Id": $scope.AMS_Id,
                    "AMSU_Id": $scope.ISMS_Id,
                }
                apiService.create("BatchwiseStudentMapping/getbatchname", data).
                    then(function (promise) {
                        if (promise.countbatchlist > 0) {
                            $scope.batchList = promise.batchList;
                        }
                        else {
                            swal("No Records Found");
                            $scope.ISMS_Id = "";
                            $scope.ASASB_Id1 = "";
                        }
                    });
            }

        }
    }
})();