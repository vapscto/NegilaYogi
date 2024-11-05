(function () {
    'use strict';
    angular.module('app').controller('TaskCreationFromClintnController', TaskCreationFromClintnController);
    TaskCreationFromClintnController.$inject = ['$rootScope', '$scope', '$state', '$location', '$q', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$window'];
    function TaskCreationFromClintnController($rootScope, $scope, $state, $location, $q, Flash, appSettings, apiService, $http, superCache, $filter, $window) {

        $scope.emplisttemplist = [];
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.fixedtime = false;
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    if (transnumconfigsettings[i].imN_Flag === "ISMTASK") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }
        }
       // $scope.obj.selfassign = 'N';

        //$scope.getserverdate = function () {
        //    var xmlHttp;
        //    function srvTime() {
        //        try {
        //            //FF, Opera, Safari, Chrome
        //            xmlHttp = new XMLHttpRequest();
        //        }
        //        catch (err1) {
        //            //IE
        //            try {
        //                xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
        //            }
        //            catch (err2) {
        //                try {
        //                    xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
        //                }
        //                catch (eerr3) {
        //                    //AJAX not supported, use CPU time.
        //                    alert("AJAX not supported");
        //                }
        //            }
        //        }
        //        xmlHttp.open('HEAD', window.location.href.toString(), false);
        //        xmlHttp.setRequestHeader("Content-Type", "text/html");
        //        xmlHttp.send('');
        //        return xmlHttp.getResponseHeader("Date");
        //    }
        //    $scope.today = srvTime();
        //}
        //$scope.getserverdate();

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.filterValue3, function (uem) {
                uem.checked = toggleStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.checkall = $scope.emplist.every(function (options) {
                return options.checked;
            });
        };

        $scope.searchValueE = '';
        $scope.filterValue2 = function (obj) {
            return angular.lowercase(obj.employeeName).indexOf(angular.lowercase($scope.searchValueE)) >= 0 || angular.lowercase(obj.hrmD_DepartmentName).indexOf(angular.lowercase($scope.searchValueE)) >= 0
                || angular.lowercase(obj.hrmdeS_DesignationName).indexOf(angular.lowercase($scope.searchValueE)) >= 0;
        };

        //var lat = 0;
        //var lan = 0;
        //var mysrclat = 0;
        //var mysrclong = 0;
        //$scope.mysrclat = '';
        //$scope.nearme = function () {
        //    if (navigator.geolocation) {
        //        navigator.geolocation.getCurrentPosition(function (position) {
        //            mysrclat = position.coords.latitude;
        //            mysrclong = position.coords.longitude;
        //            console.log("lat", mysrclat);
        //            console.log("ong", mysrclong);                 
        //            $scope.$apply(function () {
        //                $scope.lat = mysrclat;
        //                $scope.lan = mysrclong;
        //                swal("Lat lng: " + results[1].formatted_address);
        //            });                                                          
        //        });
        //    }
        //};

        //12.9845687, 77.5334751
        //12.9752688, 77.5275351
        $scope.submitted = false;
        $scope.ismtcR_CreationDate = new Date($scope.today);
        $scope.obj = {};
        $scope.taskDate = new Date($scope.today);

        $scope.ismtcR_Status = "Open";

        $scope.ondatechange = function () {
            $scope.obj.startDate = new Date($scope.today);
            //var d = new Date($scope.obj.startDate);
            //d.setDate(d.getDate() + 7);
            //$scope.obj.endDate = d;
        };

        $scope.ondatechangesta = function () {
            var d = new Date($scope.obj.startDate);
            d.setDate(d.getDate() + 5);
            $scope.obj.endDate = d;
        };

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("TaskCreationFromClint/getdetails", pageid).then(function (promise) {
                $scope.roletype = promise.roletype;
                $scope.get_department = promise.get_department;
                $scope.hrmeid = promise.hrmE_Id,
                    $scope.get_employeelist = promise.get_employeelist;
                $scope.get_priority = promise.get_priority;
                $scope.get_days = promise.get_days;
                $scope.get_taskdetails = promise.get_taskdetails;
                $scope.presentCountgrid = $scope.get_taskdetails.length;

                if (promise.plannerextapproval === true) {
                    $scope.plannerextapproval = false;
                    $scope.plannerMaxdate = promise.plannerMaxdate;
                    $scope.plMaxdate = new Date($scope.plannerMaxdate);
                    $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
                    $scope.maxdate = new Date($scope.today);
                }
                else {
                    $scope.plannerextapproval = true;
                    $scope.plMaxdate = new Date($scope.today);
                    //$scope.ismtcR_CreationDate = new Date($scope.today);
                }

                angular.forEach($scope.get_taskdetails, function (atck) {
                    atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                });
                $scope.empll = [];
                var id = 0;
                if ($scope.get_employeelist.length > 0) {
                    angular.forEach($scope.get_employeelist, function (ll) {
                        id += 1;
                        if ($scope.hrmeid === ll.HRME_Id) {
                            if ($scope.get_employeelist.length === 1) {
                                $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: true, hrmdc_id: ll.HRMDC_ID });
                            }
                            else {
                                $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: false, hrmdc_id: ll.HRMDC_ID });
                            }
                        }
                        else {
                            $scope.empll.push({ id: id, HRME_Id: ll.HRME_Id, employeeName: ll.employeeName, hrmD_DepartmentName: ll.HRMD_DepartmentName, hrmdeS_DesignationName: ll.HRMDES_DesignationName, checked: false, hrmdc_id: ll.HRMDC_ID });
                        }
                    });
                    $scope.emplist = $scope.empll;
                    $scope.emplisttemplist = $scope.empll;
                }

            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;
                $scope.reverse = !$scope.reverse;
            };
        };
       



        var get_department = [];
        var get_client = [];
         var get_project = [];
        var get_taskdetails = [];

        //$(document).ready(function () {
        //    var dataa = {};

        //    dataa = {
        //        "MI_Id": 17,
        //        "Role_flag":"ClientUser"
        //    };

        //    var stringReqdata = JSON.stringify(dataa);
        ////    console.log(stringReqdata);
        //    var urls = "https://vmsissuemanager.azurewebsites.net/api/ISM_TaskCreationFacade/getdetails/";

        //    //var urls = "https://vmsspacebooking.azurewebsites.net/api/Training_MasterFacade/details_R/";
        //    $.ajax({
        //        crossDomain: true,
        //        async: false,
        //        type: "POST",
        //        url: urls,
        //        data: stringReqdata,
        //        dataType: "json",
        //        contentType: 'application/json',
        //        context: document.body,
        //        success: function (response) {
        //            get_department = response.get_department.$values;
        //            get_client = response.get_client.$values;
        //            get_project = response.get_project.$values;
        //            get_taskdetails = response.get_taskdetails.$values;


        //            $scope.get_department = get_department;
        //            $scope.get_client = get_client;
        //            $scope.get_project = get_project;
        //            $scope.get_taskdetails = get_taskdetails;
        //        },
        //        error: function (response) {
        //            console.log(response);
        //        }
        //    });

        //});
        $scope.get_department = get_department;
        $scope.get_client = get_client;
        $scope.get_project = get_project;
        $scope.get_taskdetails = get_taskdetails;
        


        //=====================get category============
        $scope.get_category1 = function () {
            $scope.emplist = [];
            $scope.emplist = $scope.emplisttemplist;
            $scope.get_module = [];
            $scope.get_category = [];
            $scope.get_project = [];
            $scope.get_client = [];
            $scope.Module = "";
            $scope.Client = "";
            $scope.Project = "";
            $scope.Category = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id
            };
            apiService.create("TaskCreationFromClint/get_category", data).
                then(function (promise) {
                    if (promise.get_category !== null && promise.get_category.length > 0) {
                        $scope.get_category = promise.get_category;
                    }
                    else {
                        swal("No category mapped for this department!!");
                    }

                    if (promise.get_project !== null && promise.get_project.length > 0) {
                        $scope.get_project = promise.get_project;
                    }
                    else {
                        swal("No Project mapped for this department!!");
                    }
                    $scope.emplist = $filter('filter')($scope.emplist, function (d) {
                        return (d.hrmdc_id === $scope.Dept.hrmdC_ID);
                    });
                    angular.forEach($scope.emplist, function (atck) {
                        atck.checked = false;
                    });
                });
        };

        $scope.oncategorychange = function (ob) {
            $scope.obj.effmin = 0;
            $scope.obj.effhrs = 0;
            $scope.fixedtime = false;
            if (ob.ismmtcaT_EachTaskMaxDuration != undefined && ob.ismmtcaT_EachTaskMaxDuration != null && ob.ismmtcaT_EachTaskMaxDuration > 0) {
                if (ob.ismmtcaT_DurationFlg == "HOURS") {
                    $scope.obj.effhrs = parseInt(Number(ob.ismmtcaT_EachTaskMaxDuration));
                    $scope.obj.effmin = 0;
                    $scope.fixedtime = true;
                }
                if (ob.ismmtcaT_DurationFlg == "MINUTES") {
                    $scope.obj.effhrs = 0;
                    $scope.obj.effmin = parseInt(Number(ob.ismmtcaT_EachTaskMaxDuration));
                    $scope.fixedtime = true;
                }
            }
        };
        //======================================== Get Module
        $scope.onprojectChange = function () {
            $scope.get_module = [];
            $scope.get_client = [];
            $scope.Module = "";
            $scope.Client = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id,
                "roletype": $scope.roletype,
                "ISMMPR_Id": $scope.Project.ismmpR_Id
            };
            apiService.create("TaskCreationFromClint/getmodule", data).
                then(function (promise) {
                    if (promise.get_module !== null && promise.get_module.length > 0) {
                        $scope.get_module = promise.get_module;
                    }
                    else {
                        swal("No Module mapped for this Department-Project !!");
                    }
                    if (promise.get_client !== null && promise.get_client.length > 0) {
                        $scope.get_client = promise.get_client;
                    }
                    else {
                        swal("No Client mapped for this Project !!");
                    }

                });
        };
        //======================================== on client Change
        $scope.onclientchange = function () {
            $scope.get_module = [];
            $scope.Module = "";
            var data = {
                "HRMD_Id": $scope.Dept.hrmD_Id,
                "roletype": $scope.roletype,
                "ISMMPR_Id": $scope.Project.ismmpR_Id,
                "ISMMCLT_Id": $scope.Client.ismmclT_Id
            };
            apiService.create("TaskCreationFromClint/getIEuser", data).
                then(function (promise) {
                    $scope.get_IEuser = promise.get_IEuser;
                    $scope.get_module = promise.get_module;
                });
        };
        //=====================================Upload
        $scope.stepsModel = [];
        $scope.filedata = [];
        $scope.selectFileforUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i].name;
                $scope.filedata = $scope.files;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL($scope.files[i]);
            }
        };

        $scope.employeecheck = function (checkedvalue) {
            angular.forEach($scope.emplist, function (qq) {
                if (checkedvalue.id === qq.id) {
                    qq.checked = true;
                }
                else {
                    qq.checked = false;
                }
            });
        };

        //========Remove Selected File 
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.files = "";
        };
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        function UploadFiles() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                //   $scope.file_detail = $scope.files[i].name;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.files_paths = d;
                    if ($scope.files_paths.length > 0) {
                        $scope.savedata();
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //========================================= Create
        $scope.createtask = function () {
            if ($scope.files !== null && $scope.files !== undefined && $scope.files !== "") {
                UploadFiles();
            }
            else {
                $scope.savedata();
            }
        };
        $scope.obj = {};
        $scope.savedata = function (obj) {
            var data = {};
            $scope.plannerArray = [];
            $scope.taskEmpArray = [];
            $scope.files_paths = [];
            if ($scope.myForm.$valid) {
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.file !== undefined && qq.file !== null) {
                            $scope.files_paths.push({ ISMTCRAT_Attatchment: qq.file, ISMTCRAT_File: qq.FileName });
                        }
                    });
                }
                else {
                    $scope.files_paths = undefined;
                }

                data = {
                    "HRMD_Id": $scope.Dept.hrmD_Id,
                    "ISMMPR_Id": $scope.Project.ismmpR_Id,
                    "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                    "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                    "ISMTCR_Title": $scope.ismtcR_Title,
                    "ISMTCR_Desc": $scope.ismtcR_Desc,
                    "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                    "IVRMM_Id": $scope.Module.ivrmM_Id,
                    "HRMPR_Id": $scope.hrmpR_Id,
                    "ISMTCR_Status": $scope.ismtcR_Status,
                    "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                    attachmentArray: $scope.files_paths,
                    "ISMTCR_Id": $scope.ismtcR_Id,
                    "ISMCIM_IEList": $scope.ismciM_IEList,
                    "roletype": $scope.roletype,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    "assignto": $scope.obj.selfassign
                };

                if ($scope.obj.selfassign === 'Y' || $scope.ismtcR_BugOREnhancementFlg === 'T') {
                    var counthrs = 0.00;
                    var totalhrs = 0.00;
                    //if ($scope.obj.durationFlg === "HOURS") {
                    //    counthrs = parseFloat($scope.obj.eachTaskMaxDuration);
                    //} else if ($scope.obj.durationFlg === "MINUTES") {
                    //    counthrs = (parseFloat($scope.obj.eachTaskMaxDuration) * 0.0166667).toFixed(2);
                    //}

                    counthrs = (parseFloat($scope.obj.effmin) * 0.0166667).toFixed(2);
                    counthrs = parseFloat($scope.obj.effhrs) + parseFloat(counthrs);

                    angular.forEach($scope.emplist, function (ll) {
                        if (ll.checked === true) {
                            $scope.taskEmpArray.push({ HRME_Id: ll.HRME_Id });
                        }
                    });
                    if ($scope.taskEmpArray.length === 0) {
                        swal("Select any employee to assign task!!");
                        return;
                    };
                    var Yearlydatenew = null;
                    if ($scope.obj.yearly !== null && $scope.obj.yearly !== undefined) {
                        Yearlydatenew: new Date($scope.obj.yearly).toDateString();
                    }

                    if ($scope.obj.taskday === null || $scope.obj.taskday === undefined) {
                        $scope.obj.taskday = 0;
                    }
                    if (counthrs == 0 || counthrs == null) {
                        swal("Kindly enter effort for the task!!");
                        return;
                    };
                    data.effortinhrs = counthrs;
                    data.periodicity = $scope.obj.periodicity;
                    data.taskEmpArray = $scope.taskEmpArray;
                    data.remarks = $scope.obj.remarks;
                    data.startdate = new Date($scope.obj.startDate).toDateString();
                    data.enddate = new Date($scope.obj.endDate).toDateString();
                    data.TimeRequiredFlg = "HOURS";
                    data.TaskDay = $scope.obj.taskday;
                    data.Yearlydate = new Date(Yearlydatenew).toDateString();
                }

                apiService.create("TaskCreationFromClint/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Record updated successfully');
                        }
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.savedata_backup = function (obj) {
            var data = {};
            $scope.plannerArray = [];
            $scope.taskEmpArray = [];
            $scope.files_paths = [];
            if ($scope.myForm.$valid) {
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.file !== undefined && qq.file !== null) {
                            $scope.files_paths.push({ ISMTCRAT_Attatchment: qq.file, ISMTCRAT_File: qq.FileName });
                        }
                    });
                }
                else {
                    $scope.files_paths = undefined;
                }


                if ($scope.roletype === "ClientUser") {
                    data = {
                        "HRMD_Id": $scope.Dept.hrmD_Id,
                        "ISMMPR_Id": $scope.Project.ismmpR_Id,
                        "ISMMCLT_Id": $scope.Client.ismmclT_Id,
                        "ISMMTCAT_Id": $scope.Category.ismmtcaT_Id,
                        "ISMTCR_Title": $scope.ismtcR_Title,
                        "ISMTCR_Desc": $scope.ismtcR_Desc,
                        "ISMTCR_CreationDate": new Date($scope.ismtcR_CreationDate).toDateString(),
                        "IVRMM_Id": $scope.Module.ivrmM_Id,
                        "HRMPR_Id": $scope.hrmpR_Id,
                        "ISMTCR_Status": $scope.ismtcR_Status,
                        "ISMTCR_BugOREnhancementFlg": $scope.ismtcR_BugOREnhancementFlg,
                        attachmentArray: $scope.files_paths,
                        "ISMTCR_Id": $scope.ismtcR_Id,
                        "ISMCIM_IEList": $scope.ismciM_IEList,
                        "roletype": $scope.roletype,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                        "assignto": "N"
                    };
                }
               
                apiService.create("ISM_TaskCreation/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Record updated successfully');
                        }
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismtcR_Id === 0 || promise.ismtcR_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismtcR_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }

                });
            }
            else {
                $scope.submitted = true;
            }
        };



        //=====================Editrecord....
        $scope.edit = function (task) {
            $scope.ismtcR_Id = task.ismtcR_Id;
            $scope.hrmD_Id = task.hrmD_Id;
            $scope.ismmpR_Id = task.ismmpR_Id;
            $scope.ivrmM_Id = task.ivrmM_Id;
            $scope.ismtcR_BugOREnhancementFlg = task.ismtcR_BugOREnhancementFlg;
            $scope.ismtcR_CreationDate = new Date(task.ismtcR_CreationDate);
            $scope.ismtcR_Title = task.ismtcR_Title;
            $scope.hrmpR_Id = task.hrmpR_Id;
            $scope.ismtcR_Desc = task.ismtcR_Desc;
            $scope.ismtcR_Status = task.ismtcR_Status;
            $scope.files_paths = "";
            $scope.get_taskproject = [];
            $scope.get_taskmodule = [];
            $scope.get_taskclient = [];
            $scope.get_Attdetails = [];
            var data = {
                "ISMTCR_Id": task.ismtcR_Id
            };
            apiService.create("ISM_TaskCreation/gettaskAttachment", data).then(function (promise) {

                if (promise.get_taskproject.length > 0) {
                    $scope.get_taskproject = promise.get_taskproject;
                    $scope.ismmpR_Id = $scope.get_taskproject[0].ismmpR_Id;
                }
                else {
                    $scope.ismmpR_Id = 0;
                }
                if (promise.get_taskmodule.length > 0) {
                    $scope.get_taskmodule = promise.get_taskmodule;
                    $scope.ivrmM_Id = $scope.get_taskmodule[0].ivrmM_Id;
                }
                else {
                    $scope.ivrmM_Id = 0;
                }

                if (promise.get_taskclient.length > 0) {
                    $scope.get_taskclient = promise.get_taskclient;
                    $scope.Client.ismmclT_Id = $scope.get_taskclient[0].ismmclT_Id;
                }
                else {
                    $scope.Client.ismmclT_Id = 0;
                    // $scope.get_client = "";
                }
                $scope.get_Attdetails = promise.get_Attdetails;
            });
        };

        //=================Active/Deactivate
        $scope.deactive = function (task) {
            $scope.ISMTCR_Id = task.ismtcR_Id;
            var dystring = "";
            if (task.ismtcR_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (task.ismtcR_ActiveFlg === false) {
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
                        apiService.create("ISM_TaskCreation/deactive", task).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //===========Clear Field
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.getbymecount = function () {
            $scope.countflag = "ByMe";
            $scope.getscounts();
        };
        $scope.gettomecount = function () {
            $scope.countflag = "ToMe";
            $scope.getscounts();
        };
        $scope.getscounts = function () {
            $scope.get_comptasklist = [];
            $scope.get_tasks = [];
            var data = {
                "countflag": $scope.countflag
            };
            apiService.create("ISM_ComplaintsTaskList/getcount", data).
                then(function (promise) {
                    if (promise.get_tasks.length > 0) {
                        $scope.get_taskdetails = promise.get_tasks;
                        $scope.presentCountgrid = $scope.get_taskdetails.length;
                        angular.forEach($scope.get_taskdetails, function (atck) {
                            atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        });

                        //$scope.get_taskdetails = promise.get_taskdetails;
                        //$scope.presentCountgrid = $scope.get_taskdetails.length;

                        //angular.forEach($scope.get_taskdetails, function (atck) {
                        //    atck.atck = $filter('date')(atck.ISMTCR_CreationDate, "dd-MM-yyyy");
                        //});
                    }
                    else {
                        $scope.get_tasks = "";
                    }
                });
        };

        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //==============================

        //========================upload files===========
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                //var filename = input.files[0].name.toString();
                //var nameArray = filename.split('.');
                //var extention = nameArray[nameArray.length - 1];
                //document.extention = nameArray[nameArray.length - 1];
                //if (extention === "doc" || extention === "xlsx" || extention === "jpg" || extention === "jpeg" ||
                //    extention === "xls" || extention === "png"
                //    || extention === "pdf"
                //    || extention === "pptx" || extention === "ppsx" || extention === "ppt"
                //    || extention === "mp3"
                //    || extention === "mp4"
                //    || extention === "docx" || extention === "wmv") {
                //    var reader = new FileReader();
                //    reader.onload = function (e) {
                //        $('#' + document.id) //hrmedS_Id
                //            .attr('src', e.target.result);
                //    };
                //    reader.readAsDataURL(input.files[0]);
                //    UploadEmployeeDocumentOtherDetail(document);
                //}

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.file = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        //=============== view doc==========

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');
            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {
                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');
            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');
            }
            else if ($scope.filetype2 == 'pdf') {
                ///=====================show pdf, img
                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;
                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');
                    });
            }
            else {
                $window.open($scope.imagepreview)
            }
        };
        //=========================================
    }
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();

