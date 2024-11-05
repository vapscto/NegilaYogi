

    (function () {
        'use strict';
        angular
            .module('app')
            .controller('EmployeeAwardController', EmployeeAwardController)

        EmployeeAwardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
        function EmployeeAwardController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.sortReverse = true;
            //==============TO  GEt The Values iN Grid
            $scope.BindData = function () {
                var pageid = 2;
                apiService.getURI("EmployeeAward/getalldetails", pageid).
                    then(function (promise) {
                        $scope.yearlist = promise.yearlist;
                        $scope.filldepartment = promise.filldepartment;
                        if (promise.alldata.length > 0) {
                            $scope.alldatalist = promise.alldata;
                        }
                        else {
                            //swal('Records are not Found..')
                        }
                    })
            };



            //======== CheckBox Field Validation===========//
            $scope.isOptionsRequired = function () {
                return !$scope.emplist.some(function (item) {
                    return item.selected;
                });
            }

            //=======selection of checkbox....
            $scope.togchkbxC = function () {

                $scope.usercheckC = $scope.emplist.every(function (role) {
                    return role.selected;
                });
            }

            //---------all checkbox Select...
            $scope.all_checkC = function (all) {

                $scope.usercheckC = all;
                var toggleStatus = $scope.usercheckC;
                angular.forEach($scope.emplist, function (role) {
                    role.selected = toggleStatus;
                });
            }


               //=====================Department LIST
            $scope.get_department = function () {

                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                }
                apiService.create("EmployeeAward/get_depchange/", data).then(function (promise) {
                    $scope.filldesignation = promise.filldesignation;
                })
            };


            //=====================EMPLOYEE LIST
            $scope.get_emp = function () {

                debugger;
                var data = {
                    "HRMDES_Id": $scope.HRMDES_Id,
                    "HRMD_Id": $scope.HRMD_Id,
                }

                apiService.create("EmployeeAward/get_employee", data).then(function (promise) {

                    $scope.emplist = promise.emplist;

                })
            }


            //========== to Edit Data
            $scope.editrecord = function (EditRecord) {
                debugger;
                var data = {
                    "HREAW_Id": EditRecord.hreaW_Id,
                }
                
                apiService.create("EmployeeAward/editrecord/", data).
                    then(function (promise) {

                        $scope.editlist = promise.editlist;
                        $scope.filldesignation = promise.filldesignation;
                        //$scope.asmaY_Id = promise.yearlist_edit[0].asmaY_Id;         
                        $scope.asmaY_Id = promise.editlist[0].hreaW_AwardYear;         
                        $scope.hreaW_Id = promise.editlist[0].hreaW_Id;
                        $scope.HRMD_Id = promise.hrmD_Id;
                        $scope.HRMDES_Id = promise.hrmdeS_Id;
                        $scope.get_emp();
                        $scope.hrmE_Id = promise.editlist[0].hrmE_Id;
                        $scope.HREAW_AgencyName = promise.editlist[0].hreaW_AgencyName;
                        $scope.HREAW_LevelAwards = promise.editlist[0].hreaW_LevelAwards;                      

                        $scope.HREAW_AwardName = promise.editlist[0].hreaW_AwardName;
                        $scope.HREAW_AwardYear = promise.editlist[0].hreaW_AwardYear;
                        $scope.HREAW_IncentiveAmount = promise.editlist[0].hreaW_IncentiveAmount;
                        
                        $scope.file_detail = promise.editlist[0].hreaW_FileName;
                        $scope.att_file11 = promise.editlist[0].hreaW_FilePath;

                        $scope.notice = promise.editlist[0].hreaW_FilePath;
                       
                       
                        //$scope.get_department();

                        //$scope.HRMDES_Id = promise.editdata[0].hrmdeS_Id;
                        //$scope.get_emp();                      
                        

                        //angular.forEach($scope.emplist, function (ss) {
                        //    angular.forEach(promise.editdata, function (tt) {
                        //        if (ss.hrmE_Id == tt.hrmE_Id) {
                        //            ss.selected = true;
                        //        }
                        //    })
                        //})


                    })
            };

            $scope.interacted = function (field) {

                return $scope.submitted;
            };

            //========sorting
            $scope.sort = function (key) {
                $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
                $scope.sortKey = key;
            }

            //============== TO Save The Data
            $scope.submitted = false;
            $scope.saverecord = function () {
                debugger;
                $scope.emplistdata = [];
                if ($scope.myForm.$valid) {

                    //angular.forEach($scope.emplist, function (cls) {
                    //    if (cls.selected == true) {
                    //        $scope.emplistdata.push(cls);
                    //    }
                    //});
                    
                    if ($scope.notice == undefined || $scope.notice == "") {
                        var data = {
                            "HREAW_Id": $scope.hreaW_Id,
                            "HRME_Id": $scope.hrmE_Id,
                            "HREAW_AwardYear": $scope.HREAW_AwardYear,
                            "HREAW_AwardName": $scope.HREAW_AwardName,
                            "HREAW_IncentiveAmount": $scope.HREAW_IncentiveAmount,
                            "HRME_Id": $scope.hrmE_Id,
                            "HREAW_AgencyName": $scope.HREAW_AgencyName,
                            "HREAW_LevelAwards": $scope.HREAW_LevelAwards,
                            //emplstdata: $scope.emplistdata,

                        }
                    }
                    else {
                        var att_file = "";
                        $scope.filedoc = [];
                        $scope.filedoc = [$scope.notice];
                        if ($scope.filedoc.length > 0) {
                            for (var i = 0; i < $scope.filedoc.length; i++) {
                                att_file = $scope.filedoc[0];
                            }
                        }
                        var att_file11 = att_file.toString();

                        var data = {
                            "HREAW_Id": $scope.hreaW_Id,
                            "HRME_Id": $scope.hrmE_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "HREAW_AwardYear": $scope.HREAW_AwardYear,
                            "HREAW_AwardName": $scope.HREAW_AwardName,
                            "HREAW_IncentiveAmount": $scope.HREAW_IncentiveAmount,
                            "HRME_Id": $scope.hrmE_Id,
                            "HREAW_AgencyName": $scope.HREAW_AgencyName,
                            "HREAW_LevelAwards": $scope.HREAW_LevelAwards,
                            //emplstdata: $scope.emplistdata,

                            "HREAW_FileName": $scope.file_detail,
                            "HREAW_FilePath": att_file11,
                        }
                    }

                    apiService.create("EmployeeAward/saverecord", data).
                        then(function (promise) {

                            if (promise.returnval != null && promise.duplicate != null) {
                                if (promise.duplicate == false) {
                                    if (promise.returnval == true) {
                                        if ($scope.hreaW_Id > 0) {
                                            swal('Record Updated Successfully!!!');
                                            $state.reload();
                                        }
                                        else {
                                            swal('Record Saved Successfully!!!');
                                            $state.reload();
                                        }
                                    }
                                    else {
                                        if (promise.returnval == false) {
                                            if ($scope.hreaW_Id > 0) {
                                                swal('Record Not Update Successfully!!!');
                                            }
                                            else {
                                                swal('Record Not Saved Successfully!!!');
                                            }
                                        }
                                    }
                                }
                                else {
                                    swal("Record already exist");
                                }
                                $state.reload();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }

                        })
                }
                else {
                    $scope.submitted = true;
                }
            };

            /////====================================deactive and active
            $scope.deactive = function (newuser1, SweetAlertt) {
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                var mgs = "";
                if (newuser1.hreaW_ActiveFlg == false) {

                    mgs = "Activate";

                } else {
                    mgs = "Deactivate";
                }
                swal({
                    title: "Are you sure?",
                    text: "Do you want to  " + mgs + " the House Committee?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, " + mgs + " it!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("EmployeeAward/deactive", newuser1).
                                then(function (promise) {
                                    if (promise.returnval == true) {

                                        swal("Record " + mgs + "d Successfully!!!");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not " + mgs + "d Successfully!!!");
                                        $state.reload();
                                    }
                                })
                        }
                        else {
                            swal("Cancelled");
                        }
                    })
            }


           

          
            //=====================cancel
            $scope.cancel = function () {

                $scope.SPCCMH_Id = "";
                $scope.SPCCMH_Id = "";
                $scope.SPCCMHS_Description = "";
                $scope.ASMAY_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

                $scope.emplist = [];
                $scope.HRMD_Id = "";
                $scope.HRMDES_Id = "";
                angular.forEach($scope.emplist, function (itm1) {
                    itm1.selected = false;
                })

                $scope.usercheckC = "";

                $state.reload();
            }


            $scope.searchValue = "";

            

            //-----------upload file/photo.............
            $scope.uploadFile = function (input, document) {
                debugger;
                $scope.UploadFile = input.files;

                if (input.files && input.files[0]) {

                    if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {

                        var reader = new FileReader();

                        reader.onload = function (e) {
                            $('#blahD')
                                .attr('src', e.target.result)
                        };
                        reader.readAsDataURL(input.files[0]);
                        Uploadprofile();
                    }
                    else if (input.files[0].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }
            }
            function Uploadprofile() {
                debugger;
                var formData = new FormData();
                for (var i = 0; i <= $scope.uploadFile.length; i++) {

                    formData.append("File", $scope.UploadFile[i]);
                    $scope.file_detail = $scope.UploadFile[0].name;
                }
                //We can send more data to server using append         
                formData.append("Id", 0);
                var defer = $q.defer();
                $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    })
                    .success(function (d) {
                        defer.resolve(d);
                        $scope.notice = d;
                    })
                    .error(function () {
                        defer.reject("File Upload Failed!");
                    });
            }
            //----------End..........!



            ///=========clear upload field data......
            $scope.remove_file = function () {

                $scope.file_detail = "";
                $scope.notice = "";
            }

            //========EmpList CheckBox Field Validation===========//
            $scope.isOptionsRequired = function () {
                return !$scope.emplist.some(function (item) {
                    return item.selected;
                });
            }


            //=======selection of checkbox....
            $scope.togchkbxC = function () {

                $scope.usercheckC = $scope.emplist.every(function (role) {
                    return role.selected;
                });
            }


            //---------all checkbox Select...
            $scope.all_checkC = function (all) {

                $scope.usercheckC = all;
                var toggleStatus = $scope.usercheckC;
                angular.forEach($scope.emplist, function (role) {
                    role.selected = toggleStatus;
                });
            }





        }

    })();