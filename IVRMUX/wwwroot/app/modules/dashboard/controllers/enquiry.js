/// <reference path="../configuration/module.js" />

dashboard.controller('EnquiryController', EnquiryController)

EnquiryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter','superCache']
function EnquiryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {


    var paginationformasters;
    var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
    if (ivrmcofigsettings.length > 0) {
        paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
    }

    $scope.sortKey = "pasE_Id";   //set the sortKey to the param passed
    $scope.reverse = true; //if true make it false and vice versa

    //Date:23-12-2016 for displaying privileges.
    $scope.userPrivileges = "";
    var pageid = $stateParams.pageId;
    var privlgs = JSON.parse(localStorage.getItem("privileges"));
    for (var i = 0; i < privlgs.length; i++) {
        if (privlgs[i].pageId == pageid) {
            $scope.userPrivileges = privlgs[i];
        }
    }

    var configsettings = JSON.parse(localStorage.getItem("configsettings"));
    for (var i = 0; i < configsettings.length; i++) {
        if (configsettings.length > 0) {
            $scope.configurationsettings = configsettings[i];
        }
    }

    $scope.transnumbconfigurationsettings = [];
    $scope.transnumbconfigurationsettingsassign = {};
    var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
    for (var i = 0; i < transnumconfigsettings.length; i++) {
        if (transnumconfigsettings.length > 0) {
            if (transnumconfigsettings[i].imN_Flag == "Enquiry") {
                $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
            }
        }
    }

    $scope.presentCountgrid = 0;
    $scope.onSelectcountryChangecount = function (arrlistcoun) {
        var countryidd = $scope.IVRMMC_Id;
        apiService.getURI("Enquiry/getenquirycontroller", countryidd).
    then(function (promise) {
        $scope.arrliststate = promise.stateDrpDown;
    })
    }
    $scope.onSelectstateChange = function (arrliststate) {
        var countryidd = $scope.IVRMMC_Id;
        var stateid = $scope.PASE_State;
        apiService.getURI("Enquiry/getenquirystatecontroller", stateid).
        then(function (promise) {
            $scope.arrlist2 = promise.cityDrpDown;
        })
    }

    $scope.IsHidden = true;
    $scope.ShowHide = function () {
        //If DIV is hidden it will be visible and vice versa.
        $scope.IsHidden = $scope.IsHidden ? false : true;
    }


    $scope.IsHidden1 = true;
    $scope.ShowHide1 = function () {
        //If DIV is hidden it will be visible and vice versa.
        $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
    }


    $scope.IsHidden2 = true;
    $scope.ShowHide2 = function () {
        //If DIV is hidden it will be visible and vice versa.
        $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
    }

    $scope.enquiryno = false;
    $scope.enquiry = function () {

        $scope.EnquiryNo = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        var pageid = 2;

        var inputs = {
            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }

        apiService.create("Enquiry/getalldetails", inputs).
    then(function (promise) {
        $scope.arrlistcoun = promise.countryDrpDown;
        $scope.arrliststate = promise.stateDrpDown;
        //$scope.arrlist2 = promise.cityDrpDown;
        $scope.years = promise.yearDrpDwn;
        $scope.classes = promise.classDrpDwn;
        $scope.references = promise.refrenceDrpDwn;
        $scope.sources = promise.sourceDrpDwn;
        $scope.category = promise.categoryDrpDwn;
        $scope.enqname = promise.enqdata;

        $scope.presentCountgrid = promise.enqdata.length;

        $scope.ASMAY_Id = $scope.years[0].asmaY_Id;
        $scope.Enquirenorequire = false;
        // Enquiry number autogeneration
        if ($scope.transnumbconfigurationsettingsassign.imN_AutoManualFlag === "Manual") {
            $scope.EnquiryNo = true;
            $scope.Enquirenorequire = true;

        }
        else {
            $scope.EnquiryNo = false;
        }

    })
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
    $scope.search = '';
    $scope.filterOnLocation = function (user) {
        return angular.lowercase(user.pasE_FirstName + ' ' + user.pasE_MiddleName + ' ' + user.pasE_LastName).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasE_EnquiryNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasE_EnquiryDetails).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasE_emailid).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(user.pasE_MobileNo).indexOf($scope.search) >= 0 || ($filter('date')(user.pasE_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0);
    };



    $scope.searchEnq = function () {
        var inputs = {
            "id": $scope.result,
            "enquiryChoice": $scope.searchinput

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("Prospectus/getEnquiry", inputs).
         then(function (promise) {

             //$scope.arrlist2 = promise.yearDrpDwn;

             if (promise.enquiryList != null) {

                 $scope.IVRMMC_Id = promise.enquiryList[0].ivrmmC_Id;
                 $scope.PASE_MiddleName = promise.enquiryList[0].pasE_MiddleName;
                 $scope.PASE_LastName = promise.enquiryList[0].pasE_LastName;
                 $scope.PASE_EnquiryNo = promise.enquiryList[0].pasE_EnquiryNo;
                 $scope.PASE_FirstName = promise.enquiryList[0].pasE_FirstName;
                 $scope.PASE_Address1 = promise.enquiryList[0].pasE_Address1;
                 $scope.PASE_Address2 = promise.enquiryList[0].pasE_Address2;
                 $scope.PASE_Address3 = promise.enquiryList[0].pasE_Address3;
                 $scope.PASE_State = promise.enquiryList[0].pasE_State;
                 // getCityByState(promise.enqdata[0].pasE_State, promise.enqdata[0].pasE_City);
                 $scope.PASE_City = promise.enquiryList[0].pasE_City;
                 $scope.PASE_emailid = promise.enquiryList[0].pasE_emailid;
                 $scope.PASE_Pincode = promise.enquiryList[0].pasE_Pincode;
                 $scope.PASE_MobileNo = promise.enquiryList[0].pasE_MobileNo;
                 $scope.PASE_Phone = promise.enquiryList[0].pasE_Phone;

                 if ($scope.ASMAY_Id === promise.enquiryList[0].asmaY_Id) {

                 }
                 else {
                     $scope.years = promise.yearDrpDwn;
                     $scope.ASMAY_Id = promise.enquiryList[0].asmaY_Id;
                 }

                 $scope.AMC_Id = promise.enquiryList[0].amC_Id;
                 $scope.ASMCL_Id = promise.enquiryList[0].asmcL_Id;
                 $scope.PASE_Id = promise.enquiryList[0].pasE_Id;

                 $scope.PASE_EnquiryDetails = promise.enquiryList[0].pasE_EnquiryDetails;
             }
             else {
                 $scope.PASE_Id = "";

                 $state.reload();

                 $scope.enquiry();
             }
         });
    }


    $scope.DeleteEnqDetails = function (employee, SweetAlert) {
        $scope.editEmployee = employee.pasE_Id;
        var enqid = $scope.editEmployee
        swal({
            title: "Are you sure?",
            text: "Do you want to Delete this Record?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
            cancelButtonText: "Cancel!!!!!!",
            closeOnConfirm: false,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {

                var inputs = {
                    "PASE_Id": enqid,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Enquiry/DeleteDetails", inputs).
                then(function (promise) {
                   // $scope.enqname = promise.enquirydetails;

                  //  $scope.presentCountgrid = promise.enqdata.length;
                    if (promise.returnval === true) {
                        swal('Record Deleted Successfully');
                        $state.reload();
                    }
                    else {
                        swal('Record Not Deleted Successfully');
                        $state.reload();
                    }

                });
            }
            else {
                swal("Record Deletion Cancelled");
            }
        });
    }
    $scope.cance = function () {
        $state.reload();

    }
    $scope.EditDetails = function (employee) {
        $scope.editEmployee = employee.pasE_Id;
        var enqid = $scope.editEmployee;
        apiService.getURI("Enquiry/GetEnqdetails", enqid).
        then(function (promise) {
            $scope.enquiryno = true;
            $scope.IVRMMC_Id = promise.enqdata[0].ivrmmC_Id;
            $scope.PASE_MiddleName = promise.enqdata[0].pasE_MiddleName;
            $scope.PASE_LastName = promise.enqdata[0].pasE_LastName;
            $scope.PASE_EnquiryNo = promise.enqdata[0].pasE_EnquiryNo;
            $scope.PASE_FirstName = promise.enqdata[0].pasE_FirstName;
            $scope.PASE_Address1 = promise.enqdata[0].pasE_Address1;
            $scope.PASE_Address2 = promise.enqdata[0].pasE_Address2;
            $scope.PASE_Address3 = promise.enqdata[0].pasE_Address3;
            $scope.PASE_State = promise.enqdata[0].pasE_State;
            // getCityByState(promise.enqdata[0].pasE_State, promise.enqdata[0].pasE_City);
            $scope.PASE_City = promise.enqdata[0].pasE_City;
            $scope.PASE_emailid = promise.enqdata[0].pasE_emailid;
            $scope.PASE_Pincode = promise.enqdata[0].pasE_Pincode;
            $scope.PASE_MobileNo = promise.enqdata[0].pasE_MobileNo;
            $scope.PASE_Phone = promise.enqdata[0].pasE_Phone;
           
            if ($scope.ASMAY_Id === promise.enqdata[0].asmaY_Id) {

            }
            else {
                $scope.years = promise.yearDrpDwn;
                $scope.ASMAY_Id = promise.enqdata[0].asmaY_Id;
            }


            $scope.AMC_Id = promise.enqdata[0].amC_Id;
            $scope.ASMCL_Id = promise.enqdata[0].asmcL_Id;
            $scope.PASE_Id = promise.enqdata[0].pasE_Id;

            $scope.PASE_EnquiryDetails = promise.enqdata[0].pasE_EnquiryDetails;

        })
    }
    $scope.submitted = false;
    $scope.saveEnqdata = function () {
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "PASE_Id": $scope.PASE_Id,
                "AMC_Id": $scope.AMC_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "PASE_FirstName": $scope.PASE_FirstName,
                "PASE_MiddleName": $scope.PASE_MiddleName,
                "PASE_LastName": $scope.PASE_LastName,
                "PASE_Address1": $scope.PASE_Address1,
                "PASE_Address2": $scope.PASE_Address2,
                "PASE_Address3": $scope.PASE_Address3,
                "PASE_City": $scope.PASE_City,
                "PASE_State": $scope.PASE_State,
                "IVRMMC_Id": $scope.IVRMMC_Id,
                "PASE_Pincode": $scope.PASE_Pincode,
                "PASE_MobileNo": $scope.PASE_MobileNo,
                "PASE_emailid": $scope.PASE_emailid,
                "PASE_Phone": $scope.PASE_Phone,
                "PASE_EnquiryNo": $scope.PASE_EnquiryNo,
                "PASE_EnquiryDetails": $scope.PASE_EnquiryDetails,

                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                //  transnumbconfigurationsettings: $scope.transnumbconfigurationsettings

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Enquiry/", data).
                  then(function (promise) {
                      if (promise.returnval === true) {
                          if (promise.returnMsg == "a" && promise.returnduplicatestatus != "Duplicate") {
                              swal('Record Saved Successfully', 'success');
                              $state.reload();
                          }
                          else if (promise.returnduplicatestatus == "Duplicate")
                          {
                              swal('Student Name Already Exist');
                             
                          }
                          else {
                              swal('Record Updated Successfully', 'success');
                              $state.reload();
                          }
                            $state.reload();
                      }
                      else {
                          if (promise.returnMsg !=="")
                          {
                              swal(promise.returnMsg);
                              $state.reload();
                          }
                          else if (promise.returnMsg == "u") {
                              swal('Record Not Updated  Successfully', 'Failed');
                              $state.reload();
                          }
                          else if (promise.returnduplicatestatus == "Duplicate") {
                              swal('Student Name Already Exist');

                          }
                          else
                          {
                              swal('Record Not Saved Successfully', 'Failed');
                              $state.reload();
                          }
                      }
                      
                  })
        }
    };


    function getCityByState(stateId, cityId) {

        apiService.getURI("Enquiry/getenquirystatecontroller", stateId).
        then(function (promise) {
            $scope.arrlist2 = [{ "ivrmmcT_Id": 0, "ivrmmcT_Name": "--Select--" }];
            var ct = Number(cityId);
            $scope.IVRMMCT_Id = ct;
            $scope.citydata = promise.cityDrpDown;
            $scope.arrlist2.push.apply($scope.arrlist2, $scope.citydata);
        })
    }
    $scope.interacted = function (field) {

        return $scope.submitted || field.$dirty;
    };
};
