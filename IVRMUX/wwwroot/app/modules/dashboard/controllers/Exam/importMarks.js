
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamImportController', ExamImportController)
    ExamImportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function ExamImportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
        $scope.filename = '';
        var vm = this;
        vm.gridOptions = {};
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //added By sanjeev
        //var EI = this;
        //EI.gridOptions = {};
        //$scope.gridOptions = {
        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [10, 20, 30],
        //    paginationPageSize: 20,
        //    enableGridMenu: false,
        //    columnDefs: [
        //        { name: 'amsT_Id', displayName: 'AMST_Id' },
        //        { name: 'studentname', displayName: 'Student_Name' },
        //        { name: 'amsT_AdmNo', displayName: 'Admission_No' },
        //        { name: 'amaY_RollNo', displayName: 'Roll_No' },
        //        { name: 'totalMarks', displayName: 'Max_Marks' },
        //        { name: 'minMarks', displayName: 'Min_Marks' },
        //        { name: 'obtainmarks', displayName: 'Obtain_Marks' }

        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //    },
        //    gridMenuCustomItems: [{
        //        title: 'Export all data as EXCEL',
        //        action: function ($event) {
        //            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        //        },
        //        order: 110
        //    },
        //    {
        //        title: 'Export visible data as EXCEL',
        //        action: function ($event) {
        //            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
        //        },
        //        order: 111
        //    }
        //    ]

        //};
        var vm = this;
        vm.gridOptions = {};
        $scope.exportExcel = function () {
            var grid = $scope.gridApi.grid;
            var rowTypes = exportUiGridService.ALL;
            var colTypes = exportUiGridService.ALL;
            exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
        };
        $scope.rediochange = function (impall) {
            if (impall == "Import") {
                //$scope.first == false;
                // $scope.second == true;
            }
            else if (impall == "Export") {
                // $scope.first == true;
                // $scope.second == false;
                $scope.exceltable = false;
            }
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.BindData = function () {
            apiService.getDATA("ExamImport/Getdetails").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;

                })
        };
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {

            debugger;
            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }

        }
        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamImport/onselectAcdYear", data).
                then(function (promise) {
                    $scope.ctlist = promise.ctlist;
                })
        };
        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "ASMCL_Id": ASMCL_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamImport/onselectclass", data).
                then(function (promise) {
                    $scope.seclist = promise.seclist;
                })
        };
        $scope.onselectSection = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamImport/onselectSection", data).
                then(function (promise) {
                    $scope.examlist = promise.examlist;
                })
        };
        $scope.onselectExam = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id) {
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamImport/onselectExam", data).
                then(function (promise) {
                    $scope.subjectlist = promise.subjectlist;
                })
        };
        // $scope.gridOptions = {};
        $scope.onsearch = function (ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {

            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id,
                "ISMS_Id": ISMS_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamImport/onsearch", data).
                then(function (promise) {



                    $scope.gridOptions.data = promise.studentList;
                    $scope.subMorGFlag = promise.subMorGFlag;
                    $scope.gradname = promise.gradname;
                    $scope.marksdeleteflag = promise.marksdeleteflag;


                })
        };
        $scope.SaveMarks = function (gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag == "true") {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.savedata(gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false"

                            }

                        });
                }
                else {
                    $scope.savedata(gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id);
                }


            }
            else {
                swal('Entered Marks are nor correct..pease check red marks text..!');
            }
        };

        $scope.savedata = function (gridOptions, ASMS_Id, ASMCL_Id, ASMAY_Id, EME_Id, ISMS_Id) {

            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                // var clmns = EI.gridOptions.columnDefs.length;
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }
            }
            var data = {
                "ASMS_Id": ASMS_Id,
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EME_Id": EME_Id,
                "ISMS_Id": ISMS_Id,
                "newlstget": $scope.albumNameArray
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ExamImport/ImportMarks/", data).
                then(function (promise) {
                    if (promise.returnMsg != null || promise.returnMsg != "") {
                        alert(promise.returnMsg);
                    }
                    if (promise.messagesaveupdate == "true") {
                        $scope.BindData();
                        swal('Data Saved Successfully');
                    }
                    else {
                        swal('Failed to Save/Update Data');
                    }

                    $state.reload();

                })

        }
    }
})();
angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {
        debugger;
        return {
            scope: {
                opts: '='

            },
            link: function ($scope, $elm, $attrs) {

                $elm.on('change', function (changeEvent) {

                    var reader = new FileReader();

                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;

                            var workbook = XLSX.read(data, { type: 'binary' });

                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;

                            } else {
                                $scope.opts = {};

                                $scope.opts.columnDefs = [];
                                debugger;
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                //dfsd
                                $scope.opts.data = data;
                                // $rootScope.validateForm();

                                //console.log($scope.opts.data);
                                //var data1 = {
                                //    "newlstget1": $scope.opts.data
                                //}
                                //apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
                                //    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                //        swal(promise.stuStatus);
                                //        $elm.val(null);
                                //        $scope.opts.data = null;
                                //    } else {

                                //    }
                                //});
                                $scope.opts.data = $scope.opts.data;

                                //$elm.val(null);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);