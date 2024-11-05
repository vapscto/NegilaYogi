dashboard.controller("EmpDetailsImportController", ['$rootScope', '$scope', '$state', '$location', '$stateParams', 'apiService', 'dashboardService', '$timeout', '$filter', '$http', '$window', 'Flash', 'Excel', 'superCache',
function ($rootScope, $scope, $state, $location, $stateParams, apiService, dashboardService, $timeout, $filter, $http, $window, Flash, Excel, superCache) {



    var vm = this;

    $scope.hide1 = false;

    vm.gridOptions = {};
    var HostName = location.host;

    $scope.Clearid = function () {
        $state.reload();

        $scope.failedlist = [];
    }
    $scope.emp_reg = function () {
        $window.location.href = 'http://' + HostName + '/#/app/employeeRegistration/';
    }


    $scope.headerdlist = [{ id: 'EmployeeGroupType', value: 'EmployeeGroupType' },
                               { id: 'DepartmentName', value: 'DepartmentName' },
                               { id: 'DesignationName', value: 'DesignationName' },
                               { id: 'GradeName', value: 'GradeName' },
                               { id: 'EmployeeFirstName', value: 'EmployeeFirstName' },
                               { id: 'EmployeeMiddleName', value: 'EmployeeMiddleName' },

                               { id: 'EmployeeLastName', value: 'EmployeeLastName' },
                               { id: 'EmployeeCode', value: 'EmployeeCode' },
                               { id: 'Marital_Status', value: 'Marital_Status' },
                               { id: 'Gender_Name', value: 'Gender_Name' },
                               { id: 'CasteCategory_Name', value: 'CasteCategory_Name' },

                               { id: 'Caste_Name', value: 'Caste_Name' },
                                { id: 'Religion_Name', value: 'Religion_Name' },
                                { id: 'MobileNo', value: 'MobileNo' },
                                { id: 'EmailID', value: 'EmailID' }
                            ]


    $scope.reverse = false;
    $scope.order = function (keyname) {

        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }


    $scope.download_excelfile = function () {
        var exportHref = Excel.tableToExcel('#table_Id', 'WireWorkbenchDataExport');
        $timeout(function () { location.href = exportHref; }, 100); // trigger download

        //var exportHref = Excel.tableToExcel('#table_Id', 'sheet name');
        //$timeout(function () { location.href = exportHref; }, 100);
    }



    $scope.SelectedFileForUploadzd = [];

    $scope.selectFileforUploadzdd = function (input) {


        $scope.SelectedFileForUploadzd = input.files;

        if (input.files && input.files[0]) {

            var filename = input.files[0].name.toString();

            var nameArray = filename.split('.');

            var extention = nameArray[nameArray.length - 1];

            if ((extention == "xls" || extention == "xlsx"))  // 2097152 bytes = 2MB 
            {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);

                // $scope.save_data = true;
            }
            else {
                swal("Please Upload only the .xlsx/.xls file");
                return;
            }
        }


    }

    $scope.export = function (failedtable) {
        var exportHref1 = Excel.tableToExcel(failedtable, 'sheet name');
        $timeout(function () { location.href = exportHref1; }, 100);
    }



    $scope.savedata = function (gridOptions, failedtable) {

        $scope.failedlist = [];
        
        var valu = gridOptions;
        $scope.albumNameArray = [];
        if (valu.length > 0) {
            $scope.albumNameArray = [];
            var clmns = vm.gridOptions.columnDefs.length;
            for (var i = 0; i < valu.length; i++) {
                $scope.albumNameArray.push(valu[i]);
            }

        }
        var data = {

            newlstget: $scope.albumNameArray
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }

        apiService.create("EmployeeDetailsImport/", data).
        then(function (promise) {
            
            if (promise.emp_type != "") {
                swal(promise.emp_type);
                return;
            }
            else {
                vm.gridOptions = {};

                if (promise.stuStatus == "true") {
                    swal("Data Saved / Updated Successfully");
                    
                } else {

                    if (promise.failedlist != null || promise.failedlist.length > 0) {
                        
                        $scope.failedlist = promise.failedlist;
                        $scope.hide1 = true;

                        if (promise.returnMsg != null || promise.returnMsg != "")

                            swal(promise.returnMsg);

                    }
                    else {
                        $scope.failedlist = [];
                    }

                }
            }
            



        })

    }

}]).filter('keys', function () {
    
    return function (input) {
        if (!input) {
            return [];
        }

        delete input.$$hashKey;

        return Object.keys(input);

    }
})
    .factory('Excel', function ($window) {
        var uri = 'data:application/vnd.ms-excel;base64,',
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
            base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
            format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        return {
            tableToExcel: function (tableId, worksheetName) {
                var table = $(tableId),
                    ctx = { worksheet: worksheetName, table: table.html() },
                    href = uri + base64(format(template, ctx));
                return href;
            }
        };
    })
.directive("filereads", [function () {
    
    return {
        scope: {
            opts: '=',
        },
        link: function ($scope, $elm, $attrs) {

            $elm.on('change', function (changeEvent) {
                var reader = new FileReader();

                reader.onload = function (evt) {
                    $scope.$apply(function () {
                        $scope.opts = [];
                        var data = evt.target.result;

                        var workbook = XLSX.read(data, { type: 'binary' });

                     //   var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                        var headerNames = get_header_row(workbook.Sheets[workbook.SheetNames[0]]);

                        if (headerNames.length == 0) {
                            swal(' Kindly Save the file As .xls And Try again..!!');
                            $elm.val(null);
                            return;
                        }
                        
                        $scope.headerdlist = [{ id: 'EmployeeGroupType', value: 'EmployeeGroupType' },
                              { id: 'DepartmentName', value: 'DepartmentName' },
                              { id: 'DesignationName', value: 'DesignationName' },
                              { id: 'GradeName', value: 'GradeName' },
                              { id: 'EmployeeFirstName', value: 'EmployeeFirstName' },
                              { id: 'EmployeeMiddleName', value: 'EmployeeMiddleName' },

                              { id: 'EmployeeLastName', value: 'EmployeeLastName' },
                              { id: 'EmployeeCode', value: 'EmployeeCode' },
                              { id: 'Marital_Status', value: 'Marital_Status' },
                              { id: 'Gender_Name', value: 'Gender_Name' },
                              { id: 'CasteCategory_Name', value: 'CasteCategory_Name' },

                              { id: 'Caste_Name', value: 'Caste_Name' },
                              { id: 'Religion_Name', value: 'Religion_Name' },
                              { id: 'MobileNo', value: 'MobileNo' },
                              { id: 'EmailID', value: 'EmailID' }
                        ]

                        var notmatched = false;
                        angular.forEach($scope.headerdlist, function (value, key) {

                            angular.forEach(headerNames, function (value1, key1) {

                                if (value.value != value1) {
                                    notmatched = true;
                                   
                                } else {
                                    notmatched = false;
                                }
                               
                            });
                        });

                        if (notmatched) {

                            swal('Uploaded Excel Sheet is not in valid Format..!', ' Kindly download the latest Excel Format and Try again..!!');
                            $elm.val(null);
                            return;
                        } else {

                            
                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;

                                return;

                            }
                            else {

                                $scope.opts.columnDefs = [];
                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });

                                });


                                //$scope.tables = [];
                                //
                                //angular.forEach(data, function (value, key) {

                                //    $scope.tables.push({ rows: value, cols: Object.keys($scope.headerdlist) });
                                   
                                //});


                                $scope.opts.data = data;
                                
                                //for (var i = 0; i < data.length; i++) {
                                //    var chk_date = angular.isDate(data[i].HRME_DOB)
                                //    if (chk_date == true)
                                //    {
                                //        $scope.opts.data = data;
                                //    }
                                //    else
                                //    {
                                //        swal('HRME_DOB column is not valid date')
                                //    }
                                //}


                                //$elm.val(null);
                            }


                        }

                        
                    });

                };

                reader.readAsBinaryString(changeEvent.target.files[0]);

            });

            function get_header_row(sheet) {
                var headers = [];
                var range = XLSX.utils.decode_range(sheet['!ref']);
                var C, R = range.s.r; /* start in the first row */
                /* walk every column in the range */
                for (C = range.s.c; C <= range.e.c; ++C) {
                    var cell = sheet[XLSX.utils.encode_cell({ c: C, r: R })] /* find the cell in the first row */

                    var hdr = "UNKNOWN " + C; // <-- replace with your desired default 
                    if (cell && cell.t) hdr = XLSX.utils.format_cell(cell);

                    headers.push(hdr);
                }
                return headers;
            }

        }



    }
}]);
