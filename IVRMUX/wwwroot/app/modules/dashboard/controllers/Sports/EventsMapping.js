(function () {
    'use strict';

    angular
        .module('app')
        .controller('EventsMapping', EventsMapping);

    EventsMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];

    function EventsMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.validateTomintime_24 = function (timedata) {

            $scope.TRTOB_ToTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse3 = !$scope.reverse3;
        }
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";
        $scope.currentPage3 = 1;


        $scope.loaddata = function () {
            apiService.getURI("EventVenueMapping/getDetails/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;                
                $scope.eventsList = promise.eventsList;
                $scope.venuelist = promise.venuelist;
                $scope.sponsorList = promise.sponsorList;

                $scope.eventmappingList = promise.eventmappingList;
               
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.sponsorList.every(function (role) {
                return role.selected;
            });
        }

        //---------all checkbox Select...
        $scope.all_checkC = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.sponsorList, function (role) {
                role.selected = toggleStatus;
            });
        }

        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.sponsorList.some(function (item) {
                return item.selected;
            });
        }



        //========================================Save
        $scope.submitted = false;
        $scope.sponsordata1 = [];
        $scope.saveRecord = function () {

            if ($scope.spccE_SponsorFlag == '1') {
                $scope.spccE_SponsorFlag = true;
            } else {
                $scope.spccE_SponsorFlag = false;
            }
           
            if ($scope.myForm.$valid) {
                debugger;                

                $scope.spccE_StartTime = $filter('date')($scope.spccE_StartTime, "HH:mm");
                $scope.spccE_EndTime = $filter('date')($scope.spccE_EndTime, "HH:mm");

                var startdate = $scope.spccE_StartDate == null ? "" : $filter('date')($scope.spccE_StartDate, "yyyy-MM-dd");
                var enddate = $scope.spccE_EndDate == null ? "" : $filter('date')($scope.spccE_EndDate, "yyyy-MM-dd");

                //var timefrom = new Date();
                //temp = $scope.SPCCE_StartTime;
                //timefrom.setHours((parseInt(temp[0]) - 1 + 24) % 24);
                //timefrom.setMinutes(parseInt(temp[1]));

                //var timeto = new Date();
                //temp = $scope.SPCCE_EndTime;
                //timeto.setHours((parseInt(temp[0]) - 1 + 24) % 24);
                //timeto.setMinutes(parseInt(temp[1]));

                //if (timeto < timefrom)
                //    alert('start time should be smaller');


                //if ($scope.SPCCE_EndTime <= $scope.SPCCE_StartTime) {
                //    swal("End time should be greater than Start time");
                //    $scope.SPCCE_EndTime = "";
                //    $scope.loaddata();
                //}
                //else {

                $scope.sponsordata1 = [];
                angular.forEach($scope.sponsorList, function (cls) {
                    if (cls.selected == true) {
                        $scope.sponsordata1.push(cls);
                    }
                });

                    var obj = {
                        "SPCCE_Id": $scope.spccE_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "SPCCME_Id": $scope.spccmE_Id,
                        "SPCCMEV_Id": $scope.spccmeV_Id,
                        "SPCCE_StartDate": startdate,
                        "SPCCE_EndDate": enddate,
                        "SPCCE_StartTime": $scope.spccE_StartTime,                       
                        "SPCCE_EndTime": $scope.spccE_EndTime,
                        "SPCCE_Remarks": $scope.spccE_Remarks,
                        "SPCCE_SponsorFlag": $scope.spccE_SponsorFlag,
                        "sponsordata": $scope.sponsordata1,
                    }
                    apiService.create("EventVenueMapping/saveRecord", obj).then(function (promise) {
                        if (promise.returnVal == 'saved') {
                            swal("Record Saved Successfully");
                            //$scope.loaddata();
                            $state.reload();
                        }
                        else if (promise.returnVal == 'updated') {
                            swal("Record Updated Successfully");
                            //$scope.loaddata();
                            $state.reload();
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.returnVal == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.returnVal == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                    });
               // }
            }
            else {
                $scope.submitted = true;
            }
        }

        //==================================================editdata
        $scope.EditDetails = function (event) {          
            var data = {
                "SPCCE_Id": event.spccE_Id,
                "ASMAY_Id": event.asmaY_Id,
            }
            //$scope.SPCCE_Id = data;
            apiService.create("EventVenueMapping/EditDetails", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                if (promise.editDetails.length > 0) {
                    $scope.spccE_Id = promise.editDetails[0].spccE_id;
                    $scope.asmaY_Id = promise.editDetails[0].asmaY_Id;
                    $scope.spccmeV_Id = promise.editDetails[0].spccmeV_Id;
                    $scope.spccE_StartDate = new Date(promise.editDetails[0].spccE_StartDate);                   
                    $scope.spccE_EndDate = new Date(promise.editDetails[0].spccE_EndDate);
                    $scope.spccE_Remarks = promise.editDetails[0].spccE_Remarks;
                    $scope.spccmE_Id = promise.editDetails[0].spccmE_Id;
                    $scope.spccE_SponsorFlag = promise.editDetails[0].spccE_SponsorFlag;

               if (promise.editDetails[0].spccE_StartTime == null || promise.editDetails[0].spccE_StartTime == undefined || promise.editDetails[0].spccE_StartTime == "") {
                       
                    }
                    else {
                        $scope.spccE_StartTime = moment(promise.editDetails[0].spccE_StartTime, 'h:mm a').format();
                    }
                    if (promise.editDetails[0].spccE_EndTime == null || promise.editDetails[0].spccE_EndTime == undefined || promise.editDetails[0].spccE_EndTime == "") {
                        
                    }
                    else {
                        $scope.spccE_EndTime = moment(promise.editDetails[0].spccE_EndTime, 'h:mm a').format();
                    }

                    
                   //$scope.spccmsP_Id = promise.editDetails[0].spccmsP_Id;
                    if (promise.editDetails[0].spccE_SponsorFlag == false) {

                    }
                    else {
                        $scope.spccE_SponsorFlag == true;  
                    }
                }
                angular.forEach($scope.sponsorList, function (ss) {
                    angular.forEach($scope.editDetails, function (tt) {
                        if (tt.spccmsP_Id == ss.spccmsP_Id) {
                            ss.selected = true;
                        }
                    })
                })
              
            });
        }


        //=========================================================deactiveY
        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.spccE_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EventVenueMapping/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
//===========================================================================


        $scope.cancel = function () {
            //$scope.SPCCE_Id = 0;
            //$scope.ASMAY_Id = "";
            //$scope.SPCCPM_Id = "";
            //$scope.SPCCMEV_Id = "";
            //$scope.SPCCE_StartDate = "";
            //$scope.SPCCE_StartTime = "";
            //$scope.SPCCE_EndDate = "";
            //$scope.SPCCE_EndTime = "";
            //$scope.SPCCE_Remarks = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //============Model click
        $scope.onmodelclick = function (id) {
            var data = {
                "SPCCE_Id": id.spccE_Id,
                "ASMAY_Id": id.asmaY_Id,
            }
            apiService.create("EventVenueMapping/get_modeldata", data).then(function (promise) {
                
                if (promise.modalsponsorlist.length > 0) {
                    $scope.modalsponsorlist = promise.modalsponsorlist;
                }
                else {
                    swal('Sponsor Not Found!!');
                }
            });
        };
        //=========================================================deactivesponsordata
        $scope.Deactivesponsor = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.spccesP_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EventVenueMapping/Deactivesponsor", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
//===========================================================================



    }
})();
