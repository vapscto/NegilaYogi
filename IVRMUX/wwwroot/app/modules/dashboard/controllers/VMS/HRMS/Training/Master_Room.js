(function () {
    'use strict';
    angular
        .module('app')
        .controller('RoomController', roomController)

    roomController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$sce', '$q']
    function roomController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $sce, $q) {

        $scope.sortKey = 'HRMR_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.materaldocuupload = [{ id: 'Materal1' }];
        
        $scope.linkdata = [{ id: 'link1' }];
        //get data


        ////Multiple File Upload

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        // Save Function For Materal Guide Upload
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_R", pageid)
                .then(function (promise) {
                    $scope.room = promise.room_list;
                    $scope.presentCountgrid = $scope.room.length;
                    $scope.building = promise.building_list;
                    $scope.floor = promise.floor_list; 
                    $scope.paidamnlist = promise.paidamnlist; 
                    $scope.students = promise.paidamnlist; 
                    $scope.students1 = promise.freeamnlist; 
                    $scope.employeelist = promise.employeelist; 
                    console.log($scope.students);
                })
        }


        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.optionToggled1 = function (user) {
            $scope.all1 = $scope.students1.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }
        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.students1, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.employeelist, function (itm) {
                itm.select1 = toggleStatus;
            });
        }

        $scope.togchkbx = function (user) {
            $scope.usercheck = $scope.employeelist.every(function (itm) { return itm.select1; })
        }



        $scope.setdefault = function (sss) {
            if (sss.defflg == true) {
                angular.forEach($scope.materaldocuupload, function (rr) {
                    if (sss.cfileid != rr.cfileid) {
                        rr.defflg = false;
                    }


                })

            }
            else {
                if (sss.defflg == false) {
                    angular.forEach($scope.materaldocuupload, function (rr) {
                      //  if (sss.cfileid = rr.cfileid) {
                            rr.defflg = false;
                       // }


                    })

                }
            }

        }
        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {
             $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.paidlst = [];
                $scope.freelst = [];
                $scope.emplst = [];
                angular.forEach($scope.students, function (rr) {
                    if (rr.checkedvalue == true) {
                        $scope.paidlst.push(rr);
                    }
                })

                angular.forEach($scope.students1, function (rr) {
                    if (rr.checkedvalue == true) {
                        $scope.freelst.push(rr);
                    }
                })

                angular.forEach($scope.employeelist, function (rr) {
                    if (rr.select1 == true) {
                        $scope.emplst.push({HRME_Id:rr.hrmE_Id});
                    }
                })

                debugger;
                var data = {
                    "HRMR_RoomName": $scope.room1,
                    "HRMB_Id": $scope.hrmB_Id,
                    "HRMF_Id": $scope.hrmF_Id,
                    "HRMR_Id": $scope.Id,
                    "HRMR_OutSideBookingFlg": $scope.HRMR_OutSideBookingFlg,
                    "HRMR_RentPerHour": $scope.HRMR_RentPerHour,
                    "HRMR_RentPerDay": $scope.HRMR_RentPerDay,
                    "HRMR_NoOfHrs": $scope.HRMR_NoOfHrs,
                    "HRMR_Capacity": $scope.HRMR_Capacity,
                    "HRMR_TypeFlag": $scope.HRMR_TypeFlag,
                    "HRMR_Desc": $scope.HRMR_Desc,
                    filelist: $scope.materaldocuupload,
                    freeemn: $scope.freelst,
                    paidemn: $scope.paidlst,
                    emp: $scope.emplst,

                }
                apiService.create("Training_Master/SaveEdit_R", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        //dropdown
        //$scope.statesel = function () {

        //    var data = {
        //        "s_id": $scope.id
        //    };

        //    apiService.create("Training_Master/getstate", data).then(function (promise) {
        //        $scope.statelist = promise.statelist;
        //    });
        //}

        $scope.cancel = function () {
            $state.reload();
        }

        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmR_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_R", pageid).
                then(function (promise) {
                    $scope.Id = promise.room_list[0].hrmR_Id;
                    $scope.room1 = promise.room_list[0].hrmR_RoomName;
                    //$scope.hrmF_FName = promise.room_list[0].hrmF_floorname;
                    //$scope.hrmB_Title = promise.room_list[0].hrmB_builname;
                    $scope.hrmB_Id = promise.room_list[0].hrmB_Id;
                    $scope.hrmF_Id = promise.room_list[0].hrmF_Id;
                    $scope.HRMR_TypeFlag = promise.room_list[0].hrmR_TypeFlag;
                    $scope.HRMR_Capacity = promise.room_list[0].hrmR_Capacity;
                    $scope.HRMR_NoOfHrs = promise.room_list[0].hrmR_NoOfHrs;
                    $scope.HRMR_RentPerDay = promise.room_list[0].hrmR_RentPerDay;
                    $scope.HRMR_RentPerHour = promise.room_list[0].hrmR_RentPerHour;
                    $scope.HRMR_OutSideBookingFlg = promise.room_list[0].hrmR_OutSideBookingFlg;
                    $scope.HRMR_Desc = promise.room_list[0].hrmR_Desc;
                    debugger;

                    if (promise.editamenitiespaid != null && promise.editamenitiespaid.length>0) {

                        angular.forEach($scope.students, function (rr) {

                            angular.forEach(promise.editamenitiespaid , function (aa) {
                                if (rr.hrmaM_Id == aa.hrmaM_Id) {
                                    rr.checkedvalue = true;
                                    rr.noofhrs = aa.noofhrs;
                                    rr.perday = aa.perday;
                                    rr.perhours = aa.perhours;
                                }

                            })
                        })
                    }

                    if (promise.editamenitiesfree != null && promise.editamenitiesfree.length > 0) {

                        angular.forEach($scope.students1, function (rr) {

                            angular.forEach(promise.editamenitiesfree, function (aa) {
                                if (rr.hrmaM_Id == aa.hrmaM_Id) {
                                    rr.checkedvalue = true;
                                   
                                }

                            })
                        })
                    }

                    if (promise.employeelistedit != null && promise.employeelistedit.length > 0) {


                        angular.forEach($scope.employeelist, function (ff) {

                            angular.forEach(promise.employeelistedit, function (zz) {

                                if (ff.hrmE_Id==zz.hrmE_Id) {
                                    ff.select1 = true;
                                }

                            })

                        })
                        
                    }


                    $scope.materaldocuupload = [{ id: 'Materal1' }];
                    if (promise.editfiles != null && promise.editfiles.length > 0) {
                        $scope.materaldocuupload = promise.editfiles;
                        angular.forEach($scope.materaldocuupload, function (ddd) {

                            var img = ddd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                            }
                        })
                    }

                })
        };

        //delete
        //$scope.deleteemp = {}
        //$scope.delete = function (del) {
        //    $scope.deleteemp = del.id;
        //    var emp = $scope.deleteemp;
        //    apiService.getURI("Emp/delete", emp).then(function (promise) {
        //        if (promise.returnval === "Delete") {
        //            swal('Record Delete Successfully');

        //        }
        //        else {
        //            wal('Record not Delete Successfully');
        //        }
        //        $scope.student = promise.emplist;
        //        $scope.presentCountgrid = $scope.student.length;
        // })
        //}



        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.hrmR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
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
                        apiService.create("Training_Master/deactivate_R", flr).
                            then(function (promise) {

                                if (promise.hrmR_ActiveFlag === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Room Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrmR_ActiveFlag === false) {
                                    swal('Master Room Activated Successfully');
                                }
                                //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadspaceBooking", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    debugger;
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    //$('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };


        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.objdata2 = [];
        $scope.viewcontact = function (obj) {
            $scope.uploadlinks = [];
            $scope.uploadlinks = [];
            $scope.objdata2 = obj;

            apiService.create("Training_Master/viewcontact", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadlinks = promise.employeelist;
                    if (promise.employeelist !== null && promise.employeelist.length > 0) {
                        $scope.uploadlinks = promise.employeelist;


                    } else {
                        $('#popup13').modal('hide');
                        swal("No Record Found");
                    }
                }
            });
        };



        $scope.objdata1
        $scope.viewamnity = function (obj) {
            $scope.uploadlinks = [];
            $scope.uploadlinks = [];
            $scope.objdata1 = obj;

            apiService.create("Training_Master/viewamnity", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadlinks = promise.editamenitiesfree;
                    if (promise.editamenitiesfree !== null && promise.editamenitiesfree.length > 0) {
                        $scope.uploadlinks = promise.editamenitiesfree;


                    } else {
                        $('#popup12').modal('hide');
                        swal("No Record Found");
                    }
                }
            });
        };


        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            debugger;
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("Training_Master/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                       // $('#popup11').modal('hide');
                       // swal("No Documents Found");
                    }
                }
            });
        };


        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdf').modal('show');
                });
        };



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "HRMRFI_Id": obj.cfileid
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewdocument($scope.objdata)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        $scope.deleteamn = function (obj) {

            var data = {
                "HRMRAM_Id": obj.hrmraM_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deleteamn", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewamnity($scope.objdata1)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        $scope.deletecontact = function (obj) {

            var data = {
                "HRMRCO_Id": obj.hrmrcO_Id
            };

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deletecontact", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewcontact($scope.objdata2)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        //////end////

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();



