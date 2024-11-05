
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeInstallmentController', feeMIController)

    feeMIController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams']
    function feeMIController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $stateParams) {

        $scope.sortKey = "fmI_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "ftidD_Id";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        // var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.savedisable = true;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflg = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflg = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.disableinstallmentnme = false;
        $scope.disabletypename = false;

        var curDate = new Date();
        $scope.grigview1 = true;
        $scope.noinstall = false;//added by bala
        $scope.searchvalue = '';
        $scope.filtervalue = function (obj) {
            if (obj.fmI_ActiceFlag == true) {
                $scope.testSub = "Active";
            } else if (obj.fmI_ActiceFlag == false) {
                $scope.testSub = "Deactive";
            }
            return (angular.lowercase(obj.fmI_Name)).indexOf(angular.lowercase($scope.searchvalue)) >= 0 || JSON.stringify(obj.fmI_ActiceFlag).indexOf($scope.searchvalue) >= 0 || JSON.stringify(obj.fmI_No_Of_Installments).indexOf($scope.searchvalue) >= 0 || ($scope.testSub).indexOf($scope.searchvalue) >= 0;
        }


        $scope.academicdisable = false;
        $scope.installmenttypedisable = false;


        $scope.TextBoxChanged1 = function (installment) {
            
            $scope.noinstall = false;
            $scope.FMI_No_Of_Installments = "";
            if (installment == 1) {
                $scope.noinstall = true;
            }
            else if (installment == 2) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 3) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 12;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 4) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 4;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 5) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 6) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 2;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 7) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 24;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else if (installment == 8) {
                $scope.noinstall = false;
                $scope.grigview2 = true;
                var valu = 1;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
            }
            else {
                $scope.noinstall = false;
            }

        }

        //till here

        $scope.searchvalue1 = '';
        $scope.filtervalue1 = function (obj) {
            //return (($filter('date')(obj.ftidD_FromDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0)  || ($filter('date')(obj.ftidD_ToDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0)  || ($filter('date')(obj.ftidD_ApplicableDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.ftidD_DueDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || angular.lowercase(obj.fname)).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 ;
            //return (angular.lowercase(obj.fname)).indexOf(angular.lowercase($scope.searchvalue1)) >= 0;

            return (angular.lowercase(obj.fname)).indexOf(angular.lowercase($scope.searchvalue1)) >= 0 || ($filter('date')(obj.ftidD_FromDate, 'dd-MM-yyyy').indexOf($scope.searchvalue1) >= 0) || ($filter('date')(obj.ftidD_ToDate, 'dd-MM-yyyy').indexOf($scope.searchvalue1) >= 0) || ($filter('date')(obj.ftidD_ApplicableDate, 'dd-MM-yyyy').indexOf($scope.searchvalue1) >= 0) || ($filter('date')(obj.ftidD_DueDate, 'dd-MM-yyyy').indexOf($scope.searchvalue1) >= 0);
        }
       
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            var pageid = 2;
            apiService.getURI("FeeInstallment/getalldetails", pageid).
        then(function (promise) {
            $scope.databindDB = promise.installmentData;

            $scope.arrlist6 = promise.academicdrp;

            // $scope.arrlist6 = academicyrlst;
            // $scope.ASMAY = academicyrlst[0].asmaY_Id;

            //$scope.arrlist6 = academicyrlst;
            //$scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

            // $scope.arrlist6 = promise.academicdrp;
            $scope.arrlist7 = promise.instypesdrp;
            //  $scope.saveddatadisplay = promise.datasendhtml;
            $scope.albumNameArraynewone1 = [];
            for (var i = 0; i < promise.datasendhtml.length; i++) {
                var f = new Date(promise.datasendhtml[i].ftidD_FromDate);
                var t = new Date(promise.datasendhtml[i].ftidD_ToDate);
                var a = new Date(promise.datasendhtml[i].ftidD_ApplicableDate);
                var d = new Date(promise.datasendhtml[i].ftidD_DueDate);
                promise.datasendhtml[i].ftidD_FromDate = f;
                promise.datasendhtml[i].ftidD_ToDate = t;
                promise.datasendhtml[i].ftidD_ApplicableDate = a;
                promise.datasendhtml[i].ftidD_DueDate = d;
                $scope.albumNameArraynewone1.push(promise.datasendhtml[i]);
            }
            $scope.saveddatadisplay = $scope.albumNameArraynewone1;

            $scope.totcountfirst = $scope.databindDB.length;
            $scope.totcountsecond = $scope.saveddatadisplay.length;

            if (promise.datasendhtml.length > 0) {
                $scope.datanil = false;
            }
            else {
                $scope.datanil = true;
            }
            $scope.fmI_Id = 0;
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }
        }

        $scope.Clear = function () {
            
            //$scope.fmI_Id = 0;
            //$scope.FMI_Name = "";          
            //$scope.disableinstallmentnme = false;
            //$scope.disabletypename = false;
            //$scope.FMI_No_Of_Installments = "";
            //$scope.FMI_Installment_Type = "";
            //$scope.grigview2 = false;
            
            $state.reload();
            //$scope.submitted = false;
            //$scope.form1.$setPristine();
            //$scope.form1.$setUntouched();
            //$scope.searchvalue = "";
        };

        $scope.submitted = false;
        $scope.savedata = function (students) {
            
            if ($scope.form1.$valid) {
                $scope.myArray = [];
                angular.forEach($scope.students, function (user) {
                    $scope.myArray.push(user);
                })

                var data = {
                   "FMI_Id": $scope.fmI_Id,
                   "FMI_Name": $scope.FMI_Name,
                   "FMI_No_Of_Installments": $scope.FMI_No_Of_Installments,
                   "FMI_Installment_Type": $scope.FMI_Installment_Type,
                   "FMI_ActiceFlag": true,
                   "fydto": $scope.students,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeInstallment/", data).
                then(function (promise) {
                    if (promise.msg != "" && promise.msg != null) {
                        swal(promise.msg);
                        return;
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                        swal('Record Already Exist');
                        $scope.loaddata();
                        $scope.Clear();
                    }
                    else if (promise.returnduplicatestatus === "Save") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === "NotSave") {
                        swal('Record Not Saved Successfully');
                    }
                    else if (promise.returnduplicatestatus === "Update") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === "NotUpdate") {
                        swal('Record Not Updated Successfully');
                    }
                    else {
                        swal('Record Not Saved/Updated Successfully');
                        $state.reload();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMI_Name": $scope.search,
                "FMI_No_Of_Installments": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeInstallment/1", data).
        then(function (promise) {
            $scope.pages = promise.installmentData;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmI_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeInstallment/deletepages", pageid).
                    then(function (promise) {
                        $scope.pages = promise.installmentData;
                        $scope.loaddata();

                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted', 'Installment Already Mapped YearlyGroup/Amount Entry');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {

            $scope.editEmployee = employee.fmI_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeInstallment/getdetails", pageid).
            then(function (promise) {
                
                $scope.students = "";
                $scope.disableinstallmentnme = true;
                $scope.disabletypename = true;
                $scope.FMI_Installment_Type = promise.installmentData[0].fmI_Installment_Type;

                if ($scope.FMI_Installment_Type == 1) {
                    $scope.noinstall = true;
                }
                else {
                    $scope.noinstall = false;
                }

                $scope.FMI_Name = promise.installmentData[0].fmI_Name;
                $scope.FMI_No_Of_Installments = promise.installmentData[0].fmI_No_Of_Installments;
                $scope.forcompare = promise.installmentData[0].fmI_No_Of_Installments;
                $scope.FMI_Installment_Type = promise.installmentData[0].fmI_Installment_Type;
                if (promise.inTData.length > 0) {
                    $scope.grigview2 = true;
                    $scope.students = promise.inTData
                }
                else {
                    $scope.grigview2 = false;
                    $scope.disableinstallmentnme = false;
                    $scope.disabletypename = false;
                }
                $scope.fmI_Id = promise.installmentData[0].fmI_Id;
            })
        }

        $scope.TextBoxChanged = function () {
            if ($scope.fmI_Id != 0) {
                $scope.grigview2 = true;
                if ($scope.forcompare < $scope.FMI_No_Of_Installments) {
                    var valu = $scope.FMI_No_Of_Installments;
                    $scope.albumNameArray = [];
                    $scope.albumNameArraynew = $scope.students;
                    if (valu > 0) {
                        for (var i = 1; i <= ($scope.FMI_No_Of_Installments - $scope.forcompare) ; i++) {
                            $scope.albumNameArraynew.push({ 'ftI_Name': "" });
                        }
                        $scope.students = $scope.albumNameArraynew;
                    }
                }
            }
            else {
                $scope.grigview2 = true;
                var valu = $scope.FMI_No_Of_Installments;
                $scope.albumNameArray = [];
                if (valu > 0) {
                    $scope.albumNameArray = [];
                    for (var i = 1; i <= valu; i++) {
                        $scope.albumNameArray.push({ 'ftI_Name': "" });
                    }
                    $scope.students = $scope.albumNameArray;
                }
                else if (valu == 0) {
                    $scope.grigview2 = false;
                    swal("No Of Installments Can't be Zero ");
                    $scope.FMI_No_Of_Installments = "";
                }
                else {
                    $scope.grigview2 = false;
                }
            }
        }

        $scope.BindGrid = function (FMI_No_Of_Installments) {
            var data = {

                "valueloop": $scope.FMI_No_Of_Installments
            };
            apiService.create("FeeInstallment/GetWrittenTestMarks", data).
            then(function (promise) {
                var valu = $scope.FMI_No_Of_Installments;
                $scope.albumNameArray = [];
                for (var i = 1; i <= valu; i++) {
                    $scope.albumNameArray.push(i);
                }
                $scope.students = $scope.albumNameArray;
                $scope.students.push.apply($scope.students, promise);

            })
        }
        $scope.deactive1 = function (installmentData, SweetAlert) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var confirmmgs = "";
            if (installmentData.fmI_ActiceFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("FeeInstallment/deactivate", installmentData).
                then(function (promise) {
                    
                    $scope.loaddata();
                    $scope.MI_Id = "";
                    $scope.FMI_Name = "";
                    $scope.FMI_No_Of_Installments = "";
                    $scope.FMI_Installment_Type = "";
                    $scope.databindDB = promise.installmentData;
                    if (promise.returnvalue == "used") {
                        swal("Record already Used");
                    }
                    else if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    else {
                        if (promise.returnvalue == "used") {
                            swal("Record already Used");
                        }
                        else {
                            swal("Record " + confirmmgs + " Successfully");
                        }
                    }
                   
                    $scope.loaddata();
                });
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.BindGriddatawithcontrols = function () {
            
            var data = {
                "ASMAY_ID": $scope.ASMAY,
                "FMI_Id": $scope.selectedItem,
            };
            apiService.create("FeeInstallment/Getduedates", data).
                then(function (promise) {

                    $scope.grigviewseconftab = true;

                    $scope.dynamictable = promise;

                    if ($scope.dynamictable.length > 0) {
                        $scope.dynamictable = promise;
                    }
                    else {
                        $scope.grigviewseconftab = false;
                        swal("All records Saved for the Selected Installment");
                    }

                    // $scope.dynamictable.push.apply($scope.writtenMarks, promise);

                    //$scope.setmindate = new Date(academicyrlst[0].asmaY_From_Date);
                    //$scope.setmaxdate = new Date(academicyrlst[0].asmaY_To_Date);
                  //  alert($scope.setmindate)
                   // alert($scope.setmaxdate)
                    $scope.fromminDate = new Date(
                    $scope.setmindate.getFullYear(),
                    $scope.setmindate.getMonth(),
                    $scope.setmindate.getDate());

                    $scope.frommaxDate = new Date(
                    $scope.setmaxdate.getFullYear(),
                    $scope.setmaxdate.getMonth(),
                    $scope.setmaxdate.getDate());

                    $scope.dueminDate = new Date(
               $scope.setmindate.getFullYear(),
               $scope.setmindate.getMonth(),
               $scope.setmindate.getDate());

                    $scope.duemaxDate = new Date(
                    $scope.setmaxdate.getFullYear(),
                    $scope.setmaxdate.getMonth(),
                    $scope.setmaxdate.getDate());

                    $scope.appminDate = new Date(
          $scope.setmindate.getFullYear(),
          $scope.setmindate.getMonth(),
          $scope.setmindate.getDate());

                    $scope.appmaxDate = new Date(
                    $scope.setmaxdate.getFullYear(),
                    $scope.setmaxdate.getMonth(),
                    $scope.setmaxdate.getDate());

                    $scope.minDate = new Date(
          $scope.setmindate.getFullYear(),
          $scope.setmindate.getMonth(),
          $scope.setmindate.getDate());

                    $scope.maxDate = new Date(
                    $scope.setmaxdate.getFullYear(),
                    $scope.setmaxdate.getMonth(),
                    $scope.setmaxdate.getDate());

                }

            )
        }

        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(FromDate) > new Date(ToDate)) {
                swal('To Date should be greater than from date');

                return false;
            }
            console.log(ToDate);
            $scope.user.ftidD_DueDate = FromDate;
            $scope.dueminDate = new Date(
           $scope.user.ftidD_DueDate.getFullYear(),
           $scope.user.ftidD_DueDate.getMonth(),
           $scope.user.ftidD_DueDate.getDate() + 1);

            $scope.user.ftidD_ApplicableDate = FromDate;
            $scope.appminDate = new Date(
          $scope.user.ftidD_ApplicableDate.getFullYear(),
          $scope.user.ftidD_ApplicableDate.getMonth(),
          $scope.user.ftidD_ApplicableDate.getDate());

            $scope.ftidD_ApplicableDate11 = ToDate;
            $scope.appmaxDate = new Date(
                  $scope.ftidD_ApplicableDate11.getFullYear(),
                  $scope.ftidD_ApplicableDate11.getMonth(),
                  $scope.ftidD_ApplicableDate11.getDate());
        };



        //praveen
        $scope.acachange = function () {

            angular.forEach($scope.arrlist6, function (dd) {
                if ($scope.ASMAY == dd.asmaY_Id) {
                    $scope.setmindate = new Date(dd.asmaY_From_Date);
                    $scope.setmaxdate = new Date(dd.asmaY_To_Date);
                }
            })

            //alert($scope.setmindate)
            //($scope.setmaxdate)
            $scope.selectedItem = '';
            $scope.dynamictable = [];
            $scope.grigviewseconftab = false;
        }
    //end



        $scope.user = {};
        $scope.setTodate = function (data) {
            console.log(data);
            $scope.user.ftidD_ToDate = data;
            $scope.minDate = new Date(
           $scope.user.ftidD_ToDate.getFullYear(),
           $scope.user.ftidD_ToDate.getMonth(),
           $scope.user.ftidD_ToDate.getDate() + 1);
        }

        $scope.submitted1 = false;
        $scope.savedatadues = function (dynamictable) {
            $scope.albumNameArraynewone = [];
            var chkDatecondition = '';
            var fdate = '';
            var tdate = '';
            if ($scope.form2.$valid) {
                for (var i = 0; i < $scope.dynamictable.length; i++) {
                    var curDate = new Date();
                    fdate = dynamictable[i].ftidD_FromDate;
                    tdate = dynamictable[i].ftidD_ToDate;
                    if (new Date(fdate) > new Date(tdate)) {
                        chkDatecondition = 'NotAllowed';
                    }
                    else {
                        chkDatecondition = 'Allowed';
                    }
                }

                for (var i = 0; i < $scope.dynamictable.length; i++) {
                    var f = new Date(dynamictable[i].ftidD_FromDate).toDateString();
                    var t = new Date(dynamictable[i].ftidD_ToDate).toDateString();
                    var a = new Date(dynamictable[i].ftidD_ApplicableDate).toDateString();
                    var d = new Date(dynamictable[i].ftidD_DueDate).toDateString();
                    dynamictable[i].ftidD_FromDate = f;
                    dynamictable[i].ftidD_ToDate = t;
                    dynamictable[i].ftidD_ApplicableDate = a;
                    dynamictable[i].ftidD_DueDate = d;
                    $scope.albumNameArraynewone.push(dynamictable[i]);
                }


                if (chkDatecondition === 'NotAllowed') {
                    swal('To Date should be greater than from date');
                }
                else {

                    var data = {

                        //"FTI_Id": dynamictable.ftI_Id,
                        "temyrid": $scope.ASMAY,
                        "fidddto": $scope.albumNameArraynewone,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeInstallment/savedetailDDD", data).
                    then(function (promise) {

                        if (promise.returnvalue == "Duplicate") {
                            swal('Record Already Exist');
                            $scope.loaddata();
                            $scope.Clear1();

                        }
                        else if (promise.returnvalue == "Save") {

                            swal('Record Saved Successfully');
                            $scope.loaddata();
                            $scope.Clear1();
                        }

                        else if (promise.returnvalue == "NotSave") {

                            swal('Record Not Saved');
                            $scope.loaddata();
                            $scope.Clear1();
                        }
                        else if (promise.returnvalue == "Update") {

                            swal('Record Updated Successfully');
                            $scope.loaddata();
                            $scope.Clear1();
                        }
                        else if (promise.returnvalue == "NotUpdate") {

                            swal('Record Not Updated');
                            $scope.loaddata();
                            $scope.Clear1();
                        }
                        else {
                            swal('Record Not Saved/Updated ');
                            $scope.loaddata();
                            $scope.Clear1();
                        }
                    })
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.Clear1 = function () {
            $scope.grigviewseconftab = false;
            //$scope.ASMAY = "";
            $scope.selectedItem = "";
            $scope.dynamictable = [];
            $scope.searchvalue1 = "";
            $scope.submitted1 = false;
            $scope.form2.$setPristine();
            $scope.form2.$setUntouched();
            $scope.search = "";

            $scope.academicdisable = false;
            $scope.installmenttypedisable = false;

        };

        $scope.BindGriddatawithcontrols12 = function () {
            var data = {
                "ASMAY_ID": $scope.ASMAY,
                "FMI_Id": $scope.selectedItem,
            };
            apiService.create("FeeInstallment/Getduedates", data).
                then(function (promise) {
                    $scope.dynamictable = promise;
                })
        }
        $scope.editEmployee = {}
        $scope.deletedataY = function (employee, SweetAlert) {                      
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {          
                if (isConfirm) {
                    $scope.editEmployee = employee.ftidD_Id;
                    var pageid = $scope.editEmployee;

                    var data = {
                        "FTIDD_Id": pageid
                    };

                    apiService.create("FeeInstallment/deletepagesY", data).then(function (promise) {
                        $scope.loaddata();
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else if (promise.returnval === false && promise.returnvalexist != "Exist") {
                            swal('Record Not Deleted');
                        }
                        else if (promise.returnvalexist === "Exist") {
                            swal('Installment Already Mapped');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalueY = function (employee) {
            $scope.editEmployee = employee.ftidD_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeInstallment/getdetailsY", pageid).
            then(function (promise) {
                
                $scope.ASMAY = promise.installmentDatad[0].yrid;
                $scope.selectedItem = promise.installmentDatad[0].fmiid;
                $scope.grigviewseconftab = true;
                $scope.ftidD_Id = promise.installmentDatad[0].ftidD_Id;
                $scope.myArrayR = [];
                angular.forEach(promise.installmentDatad, function (user) {
                    var frdate = new Date(promise.installmentDatad[0].ftidD_FromDate);
                    user.ftidD_FromDate = frdate
                    var frtdate = new Date(promise.installmentDatad[0].ftidD_ToDate);
                    user.ftidD_ToDate = frtdate
                    var fradate = new Date(promise.installmentDatad[0].ftidD_ApplicableDate);
                    user.ftidD_ApplicableDate = fradate
                    var frddate = new Date(promise.installmentDatad[0].ftidD_DueDate);
                    user.ftidD_DueDate = frddate
                    $scope.myArrayR.push(user);
                })
                $scope.dynamictable = $scope.myArrayR;

                $scope.academicdisable = true;
                $scope.installmenttypedisable = true;

            })
        }


    }


    //added by bala 30/06/2017





    function getIndependentDrpDwnss() {
        apiService.getURI("FeeInstallment/getdpforyear", 2).then(function (promise) {
            // $scope.arrlist6 = promise.academicdrp;
            $scope.arrlist6 = academicyrlst;
            $scope.ASMAY = academicyrlst[0].asmaY_Id;
            $scope.arrlist7 = promise.instypesdrp;

            //// for pagination 
            //$scope.currentPage = 1;
            //$scope.itemsPerPage = 5;
            //$scope.pages = promise.academicdrp;
            // for pagination
        })
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
    //function BindGriddatawithcontrols() {
    //    var data = {
    //        "valueloop": $scope.arrlist7
    //    };
    //    apiService.create("FeeInstallment/Getduedates", data).
    //        then(function (promise) {
    //            $scope.dynamictable = promise;
    //            // $scope.dynamictable.push.apply($scope.writtenMarks, promise);
    //        })
    //}

})();