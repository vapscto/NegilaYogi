(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumnifriendrequestController', AlumnifriendrequestController)
    AlumnifriendrequestController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function AlumnifriendrequestController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.searchValue = '';
        $scope.searchValue1 = '';
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
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
     
        $scope.loaddata = function () {
            var id = 1;
            apiService.getURI("Alumni_Student_FriendRequest/getdata", id).then(function (promise) {
                if (promise.searchResult != null || promise.searchResult > 0) {
                    $scope.searchResult1 = [];
                    $scope.searchResult = [];
                    $scope.searchResult1 = promise.searchResult;
                    $scope.friendrequest = promise.friendrequest;
                    angular.forEach($scope.searchResult1, function (aa) {
                        if (aa.ALSFRND_RequestDate != null) {
                            $scope.searchResult.push({
                                studentname: aa.studentname,
                                ASMAY_Year: aa.ASMAY_Year,
                                ALSFRND_RequestDate: aa.ALSFRND_RequestDate,
                                ALSFRND_AcceptedDate: aa.ALSFRND_AcceptedDate,
                                checktru: true,
                                ALMST_Id: aa.ALMST_Id,
                                ALSFRND_AcceptFlag: aa.ALSFRND_AcceptFlag,
                             
                            });
                        }
                        else {
                          
                            $scope.searchResult.push({
                                studentname: aa.studentname,
                                ASMAY_Year: aa.ASMAY_Year,
                                ALSFRND_RequestDate: aa.ALSFRND_RequestDate,
                                ALSFRND_AcceptedDate: aa.ALSFRND_AcceptedDate,
                                checktru: false,
                                ALMST_Id: aa.ALMST_Id,
                                ALSFRND_AcceptFlag: aa.ALSFRND_AcceptFlag,
                            });
                           
                        }
                    })
                }
            })
        }

        $scope.imgname = logopath;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;

        $scope.grid_flag = false;
        $scope.tadprint = false;
        $scope.items = {};
        $scope.result = [
            {
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                   
                ],
                "fields": [
                    { "value": "StudentName", "name": "Alumni Name" },
                   // { "value": "ASMAY_Id_Left", "name": "MobileNo" },
                    
                ],
                //"condition": [
                //    { "value": "AND", "name": "AND" },
                //    { "value": "OR", "name": "OR" },
                //]

            }]
        $scope.addNew = function (index) {
            $scope['condflag' + index] = true;
            $scope.result.push({
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    //{ "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "StudentName", "name": "Alumni Name" },
                   
                ],
                "condition": [
                  { "value": "AND", "name": "AND" },
                   { "value": "OR", "name": "OR" }
                ]
            });
        };
        $scope.removeRow = function (index) {

            $scope.result.pop();
            $scope['condflag' + (index - 1)] = false;
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
            if ($scope.items.oprtr != '' && $scope.items.oprtr != null) {
                $scope.items.oprtr[index] = '';
            }
            if ($scope.items.field != '' && $scope.items.field != null) {
                $scope.items.field[index] = '';
            }
            if ($scope.items.conditn != '' && $scope.items.conditn != null) {
                $scope.items.conditn[index] = '';
            }
        }
        var abc = "";
        $scope.minall = abc;
        $scope.filterOperator = function (field, index) {


            if (field == "StudentName") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
           
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
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
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.searchStud = function (inputs) {

         
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var output = [];
                for (var key in inputs.field) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.field[key];
                    output.push(tempObj);
                }

                var output1 = [];
                for (var key in inputs.oprtr) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.oprtr[key];
                    output1.push(tempObj);
                }

                var output2 = [];
                for (var key in inputs.val) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.val[key];
                    output2.push(tempObj);
                }

                //var output3 = [];
                //for (var key in inputs.conditn) {
                //    // must create a temp object to set the key using a variable
                //    var tempObj = {};
                //    tempObj[key] = inputs.conditn[key];
                //    output3.push(tempObj);
                //}

                var data = {
                    output,
                    output1,
                    output2,
                   // output3,

                    
                };

               
                apiService.create("Alumni_Student_FriendRequest/getsearch_data", data).
                    then(function (promise) {
                        if (promise.count == 0) {
                            swal("No Records Found.....!!");
                            $scope.grid_flag = false;
                            $state.reload();
                        }
                        else {
                            $scope.searchResult = promise.searchResult;
                          
                        }
                    })
            };
        }
        $scope.viewData = function (ww) {
            var data = {
                "ALMST_Id":ww.ALMST_Id
            }
            apiService.create("Alumni_Student_FriendRequest/viewprofile", data).then(function (promise) {
                if (promise.almst_profile != null || promise.almst_profile > 0) {
                    $scope.alumniname1 = promise.almst_profile[0].alumniname;
                    $scope.almst_profile = promise.almst_profile;

                    $('#profileview').modal('show');
                }
                else {
                    swal('No Data Found..!!!')
                }
                
            })
            
        }

        $scope.previewimg = function (img) {
            if (img != 'undefined' || img != null) {
                $('#preview').attr('src', img);
                $('#myimagePreview').modal('show');
            }
            else {
                swal('No Image Found..!!')
            }
         
        };

        $scope.Clearid = function () {
            $state.reload();
            // $scope.radioValue = "";
            // $scope.items = "";
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.searchResult, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }
    
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.sendrequest = function () {
            $scope.printstudents1 = [];
            angular.forEach($scope.printstudents, function (aa) {
                $scope.printstudents1.push({ ALMST_Id: aa.ALMST_Id})
            })
           
            var data = {
                requestalumni: $scope.printstudents1
            }
            apiService.create("Alumni_Student_FriendRequest/sendrequest", data).then(function (promise) {
                if (promise.message == 'Success') {
                    swal('Request Sent Successfully...')
                }
                else {
                    swal('Request Not Sent..!!')
                }
                $state.reload();
            })
        }
    }
})
    ();