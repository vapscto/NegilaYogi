(function () {
    'use strict';
    angular
        .module('app')
        .controller('Master_MessController', Master_MessController)

    Master_MessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function Master_MessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.submitted = false;
        //$scope.radio_flag = "Boys";

        //=====================Load--data.............\\

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("HS_Master/get_Mmessdata", pageid).then(function (promise) {
                $scope.get_messCategorylist = promise.get_messCategorylist;

                $scope.get_messlist = promise.get_messlist;

            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        //=====================End-----Load--data----//


        //=====================saverecord....
        $scope.savedata = function () {


            $scope.selectedFlag = [];
            if ($scope.myForm.$valid) {

                $scope.bk_st_time = $filter('date')($scope.bk_st_time, "HH:mm");
                $scope.bk_end_time = $filter('date')($scope.bk_end_time, "HH:mm");
                $scope.lunch_st_time = $filter('date')($scope.lunch_st_time, "HH:mm");
                $scope.lunch_end_time = $filter('date')($scope.lunch_end_time, "HH:mm");
                $scope.lt_st_time = $filter('date')($scope.lt_st_time, "HH:mm");
                $scope.lt_end_time = $filter('date')($scope.lt_end_time, "HH:mm");
                $scope.dinner_st_time = $filter('date')($scope.dinner_st_time, "HH:mm");
                $scope.dinner_end_time = $filter('date')($scope.dinner_end_time, "HH:mm");

                var data = {
                    "HLMM_Id": $scope.hlmM_Id,
                    "HLMM_Name": $scope.hlmM_Name,
                    //"HLMMC_Id": $scope.hlmmC_Id,
                    "HLMM_VegFlg": $scope.hlmM_VegFlg,
                    "HLMM_NonVegFlg": $scope.hlmM_NonVegFlg,

                    "HLMM_BFSStartTime": $scope.bk_st_time,
                    "HLMM_BFSEndTime": $scope.bk_end_time,

                    "HLMM_LNStartTime": $scope.lunch_st_time,
                    "HLMM_LNEndTime": $scope.lunch_end_time,

                    "HLMM_LNTSStartTime": $scope.lt_st_time,
                    "HLMM_LNTSEndTime": $scope.lt_end_time,

                    "HLMM_DNSStartTime": $scope.dinner_st_time,
                    "HLMM_DNSEndTime": $scope.dinner_end_time,


                }

                apiService.create("HS_Master/save_Mmessdata", data).then(function (promise) {

                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate === false) {
                            if (promise.returnval === true) {
                                if ($scope.hlmM_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.returnval === false) {
                                    if ($scope.hlmM_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
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
                        //$state.reload();
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
        //=====================End---saverecord....


        //=====================Editrecord....
        $scope.EditData = function (user) {

            var data = {
                "HLMM_Id": user.hlmM_Id
            }
            apiService.create("HS_Master/edit_Mmessdata", data).then(function (promise) {
                $scope.edit_Messlist = promise.edit_Messlist;

                $scope.hlmM_Id = promise.edit_Messlist[0].hlmM_Id;
                $scope.hlmM_Name = promise.edit_Messlist[0].hlmM_Name;
                //$scope.hlmmC_Id = promise.edit_Messlist[0].hlmmC_Id;
                $scope.hlmM_VegFlg = promise.edit_Messlist[0].hlmM_VegFlg;
                $scope.hlmM_NonVegFlg = promise.edit_Messlist[0].hlmM_NonVegFlg;
                if ($scope.hlmM_VegFlg == true) {
                    $scope.hlmM_VegFlg = 1;
                }
                if ($scope.hlmM_NonVegFlg == true) {
                    $scope.hlmM_NonVegFlg = 1;
                }


                if (promise.edit_Messlist[0].hlmM_BFSStartTime != null || promise.edit_Messlist[0].hlmM_BFSStartTime != undefined || promise.edit_Messlist[0].hlmM_BFSStartTime != "") {
                    $scope.bk_st_time = moment(promise.edit_Messlist[0].hlmM_BFSStartTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_BFSEndTime != null || promise.edit_Messlist[0].hlmM_BFSEndTime != undefined || promise.edit_Messlist[0].hlmM_BFSEndTime != "") {
                    $scope.bk_end_time = moment(promise.edit_Messlist[0].hlmM_BFSEndTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_LNStartTime != null || promise.edit_Messlist[0].hlmM_LNStartTime != undefined || promise.edit_Messlist[0].hlmM_LNStartTime != "") {
                    $scope.lunch_st_time = moment(promise.edit_Messlist[0].hlmM_LNStartTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_LNEndTime != null || promise.edit_Messlist[0].hlmM_LNEndTime != undefined || promise.edit_Messlist[0].hlmM_LNEndTime != "") {
                    $scope.lunch_end_time = moment(promise.edit_Messlist[0].hlmM_LNEndTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_LNTSStartTime != null || promise.edit_Messlist[0].hlmM_LNTSStartTime != undefined || promise.edit_Messlist[0].hlmM_LNTSStartTime != "") {
                    $scope.lt_st_time = moment(promise.edit_Messlist[0].hlmM_LNTSStartTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_LNTSEndTime != null || promise.edit_Messlist[0].hlmM_LNTSEndTime != undefined || promise.edit_Messlist[0].hlmM_LNTSEndTime != "") {
                    $scope.lt_end_time = moment(promise.edit_Messlist[0].hlmM_LNTSEndTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_DNSStartTime != null || promise.edit_Messlist[0].hlmM_DNSStartTime != undefined || promise.edit_Messlist[0].hlmM_DNSStartTime != "") {
                    $scope.dinner_st_time = moment(promise.edit_Messlist[0].hlmM_DNSStartTime, 'h:mm a').format();
                }
                if (promise.edit_Messlist[0].hlmM_DNSEndTime != null || promise.edit_Messlist[0].hlmM_DNSEndTime != undefined || promise.edit_Messlist[0].hlmM_DNSEndTime != "") {
                    $scope.dinner_end_time = moment(promise.edit_Messlist[0].hlmM_DNSEndTime, 'h:mm a').format();
                }
            });
        }
        //====================End

        $scope.validateTomintime_24 = function (timedata) {

            // $scope.bk_st_time = "";
            //$scope.totime = timedata;
            //var hh = $scope.totime.getHours();
            //var mm = $scope.totime.getMinutes();
            //$scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }


        //=================Activation/Deactivation
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.hlmM_Id = user.hlmM_Id;

            var dystring = "";
            if (user.hlmM_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmM_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactiveY_Mmessdata", user).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.facilitylist, function (itm) {
                itm.selected = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.facilitylist.every(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.facilitylist.some(function (options) {
                return options.selected;
            });
        }

        $scope.facilitylist = [];
        $scope.facilitylist =
            [
                { fac_id: 1, fac_name: "Mess" },
                { fac_id: 2, fac_name: "Gym" },
                { fac_id: 3, fac_name: "Cleaning Service" },
                { fac_id: 4, fac_name: "TV" },
                { fac_id: 5, fac_name: "Library" },
                { fac_id: 6, fac_name: "AC" },
                { fac_id: 7, fac_name: "Water Purifier" },
                { fac_id: 8, fac_name: "Internet" },
                { fac_id: 9, fac_name: "Cooking" },
                { fac_id: 10, fac_name: "Laundry" },
                { fac_id: 11, fac_name: "Power backup" },
            ]


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

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

        $scope.get_statelist = function () {
            var data = {
                "IVRMMC_Id": $scope.ivrmmC_Id,
            }
            apiService.create("HS_Master/get_statedata", data).then(function (promise) {
                $scope.statelistdata = promise.statelistdata;
            })
        }


        $scope.editHosteldata = function (obj) {

            var data = {

                "HLMH_Id": obj.hlmM_Id,
            }
            apiService.create("HS_Master/edit_hostel_row", data).then(function (promise) {

                $scope.edit_hostel_row = promise.edit_hostel_row;

                $scope.hlmM_Id = promise.edit_hostel_row[0].hlmM_Id;
                $scope.hlmH_Name = promise.edit_hostel_row[0].hlmH_Name;
                $scope.ivrmmG_Id = promise.edit_hostel_row[0].ivrmmG_Id;
                $scope.hlmH_City = promise.edit_hostel_row[0].hlmH_City;
                $scope.hlmH_TotalFloor = promise.edit_hostel_row[0].hlmH_TotalFloor;
                $scope.hlmH_TotalRoom = promise.edit_hostel_row[0].hlmH_TotalRoom;
                $scope.hlmH_Desc = promise.edit_hostel_row[0].hlmH_Desc;
                $scope.hlmH_WardenName = promise.edit_hostel_row[0].hlmH_WardenName;
                $scope.hlmH_ContactNo = promise.edit_hostel_row[0].hlmH_ContactNo;
                $scope.hlmH_Pincode = promise.edit_hostel_row[0].hlmH_Pincode;
                $scope.hlmH_Address = promise.edit_hostel_row[0].hlmH_Address;

                $scope.ivrmmC_Id = promise.edit_countrylist[0].ivrmmC_Id;
                $scope.statelistdata = promise.statelistdata;
                $scope.ivrmmS_Id = promise.edit_hostel_row[0].ivrmmS_Id;

                if ($scope.edit_hostel_row[0].hlmH_MessFlg == true) {
                    $scope.hlmH_MessFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_GymFlg == true) {
                    $scope.hlmH_GymFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_CleaningServiceFlg == true) {
                    $scope.hlmH_CleaningServiceFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_TvFlg == true) {
                    $scope.hlmH_TvFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_LibraryFlg == true) {
                    $scope.hlmH_LibraryFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_ACFlg == true) {
                    $scope.hlmH_ACFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_WaterPurifierFlg == true) {
                    $scope.hlmH_WaterPurifierFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_InternetFlg == true) {
                    $scope.hlmH_InternetFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_CookingFlg == true) {
                    $scope.hlmH_CookingFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_LaundryFlg == true) {
                    $scope.hlmH_LaundryFlg = 1;
                }
                if ($scope.edit_hostel_row[0].hlmH_PowerBackupFlg == true) {
                    $scope.hlmH_PowerBackupFlg = 1;
                }
            })

        }


    }
})();

