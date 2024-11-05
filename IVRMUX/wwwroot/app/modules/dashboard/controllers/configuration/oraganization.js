

(function () {
    'use strict';
    angular
.module('app')
.controller('OrganisationController', OrganisationController)
    
    OrganisationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function OrganisationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ConSetting/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/institution/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ConSettingWizardComplete/';
        };
        if ($scope.ccodelength == undefined)
            $scope.dismobile = true;
        else
            $scope.dismobile = false;
        //$scope.options = ['Default', 'Office', 'Home'];

        $scope.options = [{ id: 1, option: "Default" }, { id: 2, option: "Office" }, { id: 3, option: "Home" }];
        $scope.options1 = [{ id: 1, option: "Default" }, { id: 2, option: "Office" }, { id: 3, option: "Home" }];
        $scope.options2 = [{ id: 1, option: "Default" }, { id: 2, option: "Office" }, { id: 3, option: "Home" }];
        //$scope.options = [{id}'Default', 'Office', 'Home'];
        //$scope.options1 = ['Default', 'Office', 'Home'];
        //$scope.options2 = ['Default', 'Office', 'Home'];
        $scope.disabled = {};
        $scope.disabled1 = {};
        $scope.disabled2 = {};

        $scope.paginate1 = "paginate1";

        //mobile add option
        $scope.mobiles = [{
            id: 'mobile1'
        }];
        //$scope.selphs1 = [{ id: 'selphone11' }];
        $scope.addNewMobile = function (moM_Flag) {
            var newItemNo = $scope.mobiles.length + 1;
            if (newItemNo <= 5) {
                $scope.mobiles.push({ 'id': 'mobile1' + newItemNo });
            }

            //var newItemNo1 = $scope.selphs1.length + 1;
            //if (newItemNo1 <= 5) {
            //    $scope.selphs1.push({ 'id': 'selphone1' + newItemNo1 });
            //}
            if (newItemNo == 4) {
                $scope.myForm.$setPristine();
            }
        };
        $scope.defchange1 = function (moM_Flag) {
            
            var newItemNo1 = $scope.mobiles.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.mobiles[i].moM_Flag == "Default") {
                    $scope.disabled1["Default"] = true;
                    break;
                }
                else {
                    $scope.disabled1["Default"] = false;
                }
            }
        }
        $scope.delmob = [];
        $scope.removeNewMobile = function (index, moM_Flag) {
            

            var newItemNo = $scope.mobiles.length - 1;
            if (newItemNo !== 0) {
                $scope.delmob = $scope.mobiles.splice(index, 1);
                if ($scope.delmob[0].moM_Flag == "Default") {
                    $scope.disabled1["Default"] = false;
                }
                else {
                    $scope.disabled1["Default"] = true;
                }

            }
            //var newItemNo1 = $scope.selphs1.length - 1;
            //if (newItemNo1 !== 0) {
            //    $scope.delmob = $scope.selphs1.splice(index, 1);

            //    if ($scope.delmob[0].moM_Flag == "Default") {
            //        $scope.disabled1["Default"] = false;
            //    }
            //    else {
            //        $scope.disabled1["Default"] = true;
            //    }
            //}
        };

        //$scope.addNewMobile = function (curval1) {
        //    if (curval1 == "Default") {
        //        $scope.disabled1[curval1] = true;
        //    }
        //    else {
        //        $scope.disabled1[curval1] = false;
        //    }
        //    var newItemNo = $scope.mobiles.length + 1;
        //    if (newItemNo <= 5) {
        //        $scope.mobiles.push({ 'id': 'mobile' + newItemNo });
        //    }
        //};
        //$scope.removeNewMobile = function (index, curval1) {
        //    $scope.disabled1[curval1] = false;
        //    var newItemNo = $scope.mobiles.length - 1;

        //    if (newItemNo !== 0) {
        //        $scope.mobiles.splice(index, 1);
        //    }
        //};
        $scope.showAddMobile = function (mobile) {
            return mobile.id === $scope.mobiles[$scope.mobiles.length - 1].id;
        };

        //email option
        $scope.emails = [{
            id: 'email1'
        }];

        //$scope.selphs2 = [{ id: 'selphone12' }];
        $scope.addNewEmail = function (moE_Flag) {
            var newItemNo = $scope.emails.length + 1;
            if (newItemNo <= 5) {
                $scope.emails.push({ 'id': 'phone' + newItemNo });
            }

            //var newItemNo1 = $scope.selphs2.length + 1;
            //if (newItemNo1 <= 5) {
            //    $scope.selphs2.push({ 'id': 'selphone' + newItemNo1 });
            //}
            if (newItemNo == 4) {
                $scope.myForm.$setPristine();
            }
        };
        $scope.defchange2 = function (moE_Flag) {
            
            var newItemNo1 = $scope.emails.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.emails[i].moE_Flag == "Default") {
                    $scope.disabled2["Default"] = true;
                    break;
                }
                else {
                    $scope.disabled2["Default"] = false;
                }
            }
        }
        $scope.delemail = [];
        $scope.removeNewEmail = function (index, moE_Flag) {
            

            var newItemNo = $scope.emails.length - 1;
            if (newItemNo !== 0) {
                $scope.delemail = $scope.emails.splice(index, 1);
                if ($scope.delemail[0].moE_Flag == "Default") {
                    $scope.disabled2["Default"] = false;
                }
                else {
                    $scope.disabled2["Default"] = true;
                }

            }
            //var newItemNo1 = $scope.selphs2.length - 1;
            //if (newItemNo1 !== 0) {
            //    $scope.delemail = $scope.selphs2.splice(index, 1);

            //    if ($scope.delemail[0].moE_Flag == "Default") {
            //        $scope.disabled2["Default"] = false;
            //    }
            //    else {
            //        $scope.disabled2["Default"] = true;
            //    }
            //}
        };
        //$scope.addNewEmail = function (curval2) {
        //    if (curval2 == "Default") {
        //        $scope.disabled2[curval2] = true;
        //    }
        //    else {
        //        $scope.disabled2[curval2] = false;
        //    }
        //    var newItemNo = $scope.emails.length + 1;
        //    if (newItemNo <= 5) {
        //        $scope.emails.push({
        //            'id': 'email' + newItemNo
        //        });
        //    }
        //};
        //$scope.removeNewEmail = function (index, curval2) {
        //    $scope.disabled2[curval2] = false;
        //    var newItemNo = $scope.emails.length - 1;
        //    // remove the row specified in index
        //    if (newItemNo !== 0) {
        //        $scope.emails.splice(index, 1);
        //    }
        //};
        $scope.showAddEmail = function (email) {
            return email.id === $scope.emails[$scope.emails.length - 1].id;
        };

        //phones add option
        $scope.phones = [{
            id: 'phone1'
        }];
        // $scope.selphs = [{ id: 'selphone1' }];
        $scope.addNewPhone = function (moP_Flag) {
            var newItemNo = $scope.phones.length + 1;
            if (newItemNo <= 5) {
                $scope.phones.push({ 'id': 'phone' + newItemNo });
            }

            //var newItemNo1 = $scope.phones.length + 1;
            //if (newItemNo1 <= 5) {
            //    $scope.selphs.push({ 'id': 'selphone' + newItemNo1 });
            //}
            if (newItemNo == 4) {
                $scope.myForm.$setPristine();
            }
        };
        $scope.defchange = function (moP_Flag) {
            
            var newItemNo1 = $scope.phones.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.phones[i].moP_Flag == "Default") {
                    $scope.disabled["Default"] = true;
                    break;
                }
                else {
                    $scope.disabled["Default"] = false;
                }
            }
        }
        $scope.del = [];
        $scope.removeNewPhone = function (index, moP_Flag) {
            

            var newItemNo = $scope.phones.length - 1;
            if (newItemNo !== 0) {
                $scope.del = $scope.phones.splice(index, 1);
                if ($scope.del[0].moP_Flag == "Default") {
                    $scope.disabled["Default"] = false;
                }
                else {
                    $scope.disabled["Default"] = true;
                }

            }
            //var newItemNo1 = $scope.phones.length - 1;
            //if (newItemNo1 !== 0) {
            //    $scope.del = $scope.phones.splice(index, 1);

            //    if ($scope.del[0].moP_Flag == "Default") {
            //        $scope.disabled["Default"] = false;
            //    }
            //    else {
            //        $scope.disabled["Default"] = true;
            //    }
            //}
        };






        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




        //$scope.showAddPhone = function (phone) {
        //    return phone.id === $scope.phones[$scope.phones.length - 1].id;
        //};

        //$scope.predicate = 'sno';
        //$scope.reverse = true;
        //$scope.currentPage = 1;
        //$scope.order = function (predicate) {
        //    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        //    $scope.predicate = predicate;
        //};



        $scope.editEmployee = {}

        $scope.filterby = function () {

            var entereddata = $scope.search;

            var data = {
                "MO_Address1": $scope.search,
                "MO_Address2": $scope.type
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("Organisation/1", data).
        then(function (promise) {
            $scope.orgname = promise.organisationname;
        })
        }

        $scope.onSelectcountryChangecount = function (arrlistcoun) {

            if ($scope.IVRMMC_Id == undefined || $scope.IVRMMC_Id == "") {
                $scope.dismobile = false;
                $scope.ccodelength = "";
                $scope.MT_Currency = "";
                $scope.phonecode = "";
            }
            else {

                $scope.dismobile = true;
                var countryidd = $scope.IVRMMC_Id;
                apiService.getURI("Organisation/getorganisationcontroller", countryidd).
            then(function (promise) {
                $scope.dismobile = false;
                $scope.arrliststate = promise.stateDrpDown;
                $scope.ccodelength = promise.ccodelength;
                $scope.MT_Currency = promise.defaultcurrency;
                $scope.phonecode = promise.ivrmmC_CountryPhCode;
            })
            }

        }

        $scope.onSelectstateChange = function (arrliststate) {
            var countryidd = $scope.IVRMMC_Id;
            var stateid = $scope.IVRMMS_Id;
            apiService.getURI("Organisation/getorganisationstatecontroller", countryidd).
        then(function (promise) {
            // $scope.arrlist2 = promise.cityDrpDown;

            // $scope.MT_Currency = promise.defaultcurrency;
        })
        }

        $scope.getcurrencydetails = function (arrlistcoun) {
            var countryidd = $scope.IVRMMC_Id;
            apiService.getURI("Organisation/getcurrencydetails", countryidd).
        then(function (promise) {
            $scope.MT_Currency = promise.defaultcurrency;
        })
        }
        $scope.organiSortPagInfo = {};
        $scope.organi = function () {
            var data = {
                trustPagination: $scope.organiSortPagInfo
            }
            apiService.create("Organisation/getalldetails/", data).
        then(function (promise) {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.arrlistcoun = promise.countryDrpDown;
            $scope.arrliststate = promise.stateDrpDown;
            // $scope.arrliststate = promise.stateDrpDown;
            //$scope.arrlist2 = promise.cityDrpDown;

            // for institution pagination 
            //$scope.currentPage = promise.trustPagination.currentPageIndex;
            //$scope.itemsPerPage = promise.trustPagination.pageSize;
            //$scope.totalitems = promise.trustPagination.totalItems;

            $scope.orgname = promise.organisationname;

            // for  institution pagination


            //$scope.totalItems = $scope.orgname.length;
            //$scope.numPerPage = 5;

        })

            $scope.ordertrust = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.deletrec = function (employee, SweetAlert) {
            $scope.editEmployee = employee.mO_Id;
            var orgaid = $scope.editEmployee
            var mgs = "";
            var confirmmgs = "";
            if (employee.mO_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
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
                    apiService.DeleteURI("Organisation/deletedetails", orgaid).
                    then(function (promise) {

                        $scope.orgname = promise.organisationname;

                        swal(promise.returnval);
                        $state.reload();

                        //if (promise.returnval === true) {
                        //    swal('Record Successfully ' + confirmmgs);
                        //}
                        //else {
                        //    swal('Record not Successfully ' + confirmmgs);
                        //}

                    })
                }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
            });
        }

        $scope.cance = function () {
            $state.reload();
        }

        $scope.getorgvalue = function (employee) {
            
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.phones = [{ id: 'phone1' }];
            $scope.emails = [{ id: 'email1' }];
            $scope.editEmployee = employee.mO_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("Organisation/getdetails", orgid).
            then(function (promise) {

                // $scope.arrliststate = promise.stateDrpDown;
                // $scope.arrlist2 = promise.cityDrpDown;
                // $scope.cance();
                $scope.MO_Id = promise.organisationname[0].mO_Id;
                $scope.IVRMMC_Id = promise.organisationname[0].ivrmmC_Id;
                $scope.IVRMMS_Id = promise.organisationname[0].ivrmmS_Id;
                $scope.IVRMMCT_Name = promise.organisationname[0].ivrmmcT_Name;
                $scope.MO_Name = promise.organisationname[0].mO_Name;
                $scope.MO_Address1 = promise.organisationname[0].mO_Address1;
                $scope.MO_Address2 = promise.organisationname[0].mO_Address2;
                $scope.MO_Address3 = promise.organisationname[0].mO_Address3;
                //$scope.MO_Landmark = promise.organisationname[0].mO_Landmark;
                $scope.MO_Pincode = promise.organisationname[0].mO_Pincode;
                $scope.MO_FaxNo = promise.organisationname[0].mO_FaxNo;
                $scope.MO_Website = promise.organisationname[0].mO_Website;
                $scope.MO_OrganisationType = promise.organisationname[0].mO_OrganisationType;

                $scope.MT_Currency = promise.organisationname[0].mT_Currency;

                $scope.MT_Domain_name = promise.organisationname[0].mT_Domain_name;

                if (promise.mobilearrayList.length > 0) {
                    $scope.mobiles = promise.mobilearrayList;

                    var newItemNo1 = $scope.mobiles.length;
                    for (var i = 0; i < newItemNo1; i++) {
                        if ($scope.mobiles[i].moM_Flag == "Default") {
                            $scope.disabled1["Default"] = true;
                            break;
                        }
                        else {
                            $scope.disabled1["Default"] = false;
                        }
                    }
                }


                if (promise.phonearrayList.length > 0) {
                    $scope.phones = promise.phonearrayList;

                    var newItemNo1 = $scope.phones.length;
                    for (var i = 0; i < newItemNo1; i++) {
                        if ($scope.phones[i].moP_Flag == "Default") {
                            $scope.disabled["Default"] = true;
                            break;
                        }
                        else {
                            $scope.disabled["Default"] = false;
                        }
                    }
                }

                if (promise.emailarrayList.length > 0) {
                    $scope.emails = promise.emailarrayList;

                    var newItemNo1 = $scope.emails.length;
                    for (var i = 0; i < newItemNo1; i++) {
                        if ($scope.emails[i].moE_Flag == "Default") {
                            $scope.disabled2["Default"] = true;
                            break;
                        }
                        else {
                            $scope.disabled2["Default"] = false;
                        }
                    }
                }

                //$scope.MOE_EmailId = promise.organisationname[0].moE_EmailId;
                //$scope.MOPN_PhoneNo = promise.organisationname[0].mopN_PhoneNo;
                //$scope.MOMN_MobileNo = promise.organisationname[0].momN_MobileNo;

                if (promise.activeflag = 0) {
                    //$scope.activeflag = function () {
                    //    $scope.activeflag.checked = true;
                    //};
                    $scope.activeflag = true;
                }
                else {
                    //$scope.activeflag = function () {
                    //    $scope.activeflag.checked = false;
                    //};
                    $scope.activeflag = false;
                }

            })
        }

        $scope.submitted = false;
        $scope.saveorgdata = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                

                var activeflag = $scope.MO_ActiveFlag
                //if (activeflag === true)
                //{
                //    activeflag = 0;
                //}
                //else
                //{
                //    activeflag = 1;
                //}

                activeflag = 1;

                var phones = $scope.phones;
                var mobiles = $scope.mobiles;
                var emails = $scope.emails;

                var data = {
                    "MO_Id": $scope.MO_Id,
                    "IVRMMCT_Name": $scope.IVRMMCT_Name,
                    "IVRMMS_Id": $scope.IVRMMS_Id,
                    "IVRMMC_Id": $scope.IVRMMC_Id,
                    "MO_Name": $scope.MO_Name,
                    "MO_Address1": $scope.MO_Address1,
                    "MO_Address2": $scope.MO_Address2,
                    "MO_Address3": $scope.MO_Address3,
                    //"MO_Landmark": $scope.MO_Landmark,
                    "MO_Pincode": $scope.MO_Pincode,
                    "MO_FaxNo": $scope.MO_FaxNo,
                    "MO_Website": $scope.MO_Website,
                    "MO_OrganisationType": $scope.MO_OrganisationType,
                    "MO_ActiveFlag": activeflag,
                    "MT_Currency": $scope.MT_Currency,
                    "MT_Domain_name": $scope.MT_Domain_name,

                    phones: phones,
                    mobiles: mobiles,
                    emails: emails,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Organisation/", data).
                then(function (promise) {
                    $scope.orgname = promise.organisationname;

                    swal(promise.returnval);
                    $state.reload();

                    //if (promise.returnval == true && promise.returnduplicatestatus != "Duplicate") {
                    //    swal('Record Saved/Updated Successfully', 'success');
                    //    $state.reload();
                    //}
                    //else if (promise.returnval == false && promise.returnduplicatestatus != "Duplicate") {
                    //    swal('Record Not Saved/Updated Successfully');
                    //}
                    //else if (promise.returnduplicatestatus == "Duplicate") {
                    //    swal('Duplicate Record');
                    //}
                })
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };



        //pagination

        //search functionality and pagination
        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;
                $scope.searchtrust();   //calling Search functionality
            }
        };


        $scope.ordertrust = function (keyname) {

            if ((keyname == "mO_Name" && $scope.reverse == undefined) || (keyname == "mO_Name" && $scope.reverse == true)) {
                $scope.sortOrder = "mO_Name_desc";
                $scope.reverse = false;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchtrust();  //calling Search functionality
            }
            else if ((keyname == "mO_Name" && $scope.reverse == false)) {
                $scope.sortOrder = "mO_Name";
                $scope.reverse = true;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchtrust();  //calling Search functionality


            }
            else if ((keyname == "mO_OrganisationType" && $scope.reverse == undefined) || (keyname == "mO_OrganisationType" && $scope.reverse == true)) {
                $scope.sortOrder = "mO_OrganisationType_desc";
                $scope.reverse = false;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchtrust();  //calling Search functionality


            }
            else if (keyname == "mO_OrganisationType" && $scope.reverse == false) {
                $scope.sortOrder = "mO_OrganisationType";
                $scope.reverse = true;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchtrust();  //calling Search functionality
            }
        }

        // for institute Search pagination
        $scope.inst = {};
        $scope.searchtrust = function () {

            $scope.trustSortPagInfo = {
                sortOrder: $scope.sortOrder,
                CurrentPageIndex: $scope.newPage,
                searchString: $scope.inst.searchString,
                searchType: $scope.inst.searchType
            };

            apiService.create("Organisation/getOrganisationSearchedDetails", $scope.trustSortPagInfo).
               then(function (promise) {
                   $scope.trustSortPagInfo = {};
                   // for institution pagination 
                   $scope.inst = {};
                   if (promise.organisationname != null && promise.organisationname.length > 0) {

                       $scope.currentPage = promise.trustPagination.currentPageIndex;
                       $scope.itemsPerPage = promise.trustPagination.pageSize;
                       $scope.totalitems = promise.trustPagination.totalItems;

                       $scope.orgname = promise.organisationname;

                   }
                   else {
                       $scope.inst = {};
                       swal('No Records found to display..!', '');
                   }

                   // for  institution pagination
               });
        }

        //pagination
    }

})();