(function () {
    'use strict';
    angular.module('app').controller('LMSCollegeStudentcontroller', LMSCollegeStudentcontroller)

    LMSCollegeStudentcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$sce']
    function LMSCollegeStudentcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $sce) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.details = false;
        $scope.btnshow = true;
       
        //TO  GEt The Values iN Grid
        $scope.currentPage = 1;


        $scope.colorlist = [
            { id: 1, color: "linear-gradient(to bottom, #a8c0ff, #3f2b96)" },
            { id: 2, color: "linear-gradient(to bottom, #4e54c8, #8f94fb)" },
            { id: 3, color: "linear-gradient(to bottom, #667db6, #0082c8, #0082c8, #667db6)" },
            { id: 4, color: "linear-gradient(to top, #4b6cb7, #182848)" },
            { id: 5, color: "linear-gradient(to top, #7474bf, #348ac7)" },
            { id: 6, color: "linear-gradient(to top, #24c6dc, #514a9d)" },
            { id: 7, color: "linear-gradient(to top, #5c258d, #4389a2)" },
            { id: 8, color: "linear-gradient(to top, #085078, #85d8ce)" },
            { id: 9, color: "linear-gradient(to top, #4776e6, #8e54e9)" },
            { id: 10, color: "linear-gradient(to top, #614385, #516395)" },
            { id: 11, color: "linear-gradient(to top, #dd5e89, #f7bb97)" },
            { id: 12, color: "linear-gradient(to top, #1a2980, #26d0ce)" },
            { id: 13, color: "linear-gradient(to top, #1488cc, #2b32b2)" },
            { id: 14, color: "linear-gradient(to top, #5433ff, #20bdff)" },
            { id: 15, color: "linear-gradient(to top, #0052d4, #8394e2)" },
            { id: 16, color: "linear-gradient(to top, #2193b0, #6dd5ed)" },
            { id: 17, color: "linear-gradient(to top, #8360c3, #2ebf91)" },
            { id: 18, color: "linear-gradient(to top, #654ea3, #eaafc8)" },
            { id: 19, color: "linear-gradient(to top, #00b4db, #096180)" },
            { id: 20, color: "linear-gradient(to top, #a798c3, #5d26c1)" },
            { id: 21, color: "linear-gradient(to bottom, #005aa7, #e4e1bc)" },
            { id: 22, color: "linear-gradient(to top, #fc5c7d, #6a82fb)" },
            { id: 23, color: "linear-gradient(to top, #a1e0ba, #0d5fb1)" }
        ];

        $scope.mainunitdetails = [];
        $scope.maintopicdetails = [];
        $scope.topicdetails = [];
        $scope.getdocumentlistnew = [];
        $scope.uploaddocfiles = [];
        $scope.getsubjectdetailsnew = [];
        $scope.getsubjectdetails = [];

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("LMSStudent/Getdetails", id).then(function (promise) {
                $scope.mastersemester = promise.getsemesterdetails;
                $scope.currentsemester = promise.getcurrentsemesterdetails;
                angular.forEach($scope.mastersemester, function (ys) {
                    angular.forEach($scope.currentsemester, function (cs) {
                        if (ys.amsE_Id === cs.amsE_Id) {
                            ys.Selected = true;
                            $scope.AMSE_Id = $scope.currentsemester[0].amsE_Id;
                            $scope.classname = $scope.currentsemester[0].amsE_SEMName;
                        }
                    });
                });

                $scope.getstudentdetails = promise.getstudentdetails;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {
                    $scope.getsubjectdetailsnew = promise.getsubjectdetails;
                    if ($scope.getsubjectdetailsnew !== null && $scope.getsubjectdetailsnew.length > 0) {
                        $scope.getsubjectdetails = $scope.getsubjectdetailsnew;

                        angular.forEach($scope.getsubjectdetails, function (dd, i) {
                            angular.forEach($scope.colorlist, function (clr, j) {
                                if (i === j) {
                                    dd.colorname = clr.color;
                                }
                            });
                        });
                    } else {
                        swal("Subjects Details Not Found");
                    }
                } else {
                    swal("Students Details Not Found");
                }
            });
        };

        $scope.onchangesemester = function () {

            $scope.mainunitdetails = [];
            $scope.maintopicdetails = [];
            $scope.getdocumentlistnew = [];
            $scope.topicdetails = [];
            $scope.uploaddocfiles = [];

            $scope.getsubjectdetailsnew = [];
            $scope.getsubjectdetails = [];

            var data = {
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("LMSStudent/onchangesemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsubjectdetailsnew = promise.getsubjectdetails;
                    if ($scope.getsubjectdetailsnew !== null && $scope.getsubjectdetailsnew.length > 0) {
                        $scope.getsubjectdetails = $scope.getsubjectdetailsnew;

                        angular.forEach($scope.getsubjectdetails, function (dd, i) {
                            angular.forEach($scope.colorlist, function (clr, j) {
                                if (i === j) {
                                    dd.colorname = clr.color;
                                }
                            });
                        });

                        angular.forEach($scope.mastersemester, function (dd) {
                            if (dd.amsE_Id === parseInt($scope.AMSE_Id)) {
                                $scope.classname = dd.amsE_SEMName;
                            }
                        });                      

                    } else {
                        swal("Subjects Details Not Found");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };


        $scope.showSubjectdiv = function () {
            $scope.mainunitdetails = [];
            $scope.maintopicdetails = [];
            $scope.topicdetails = [];
            $scope.getdocumentlistnew = [];
            $scope.uploaddocfiles = [];
            $scope.btnshow = true;
            $("#divsubjects").show();
        };

        $scope.gettopics = function (obj) {

            $scope.mainunitdetails = [];
            $scope.maintopicdetails = [];
            $scope.topicdetails = [];
            $scope.getdocumentlistnew = [];
            $scope.uploaddocfiles = [];

            var data = {
                "AMSE_Id": $scope.AMSE_Id,
                "ISMS_Id": obj.ismS_Id
            };
            apiService.create("LMSStudent/getcollegetopics", data).then(function (promise) {
                if (promise !== null) {

                    $scope.getunitdetails = promise.getunitdetails;
                    $scope.gettopiclist = promise.gettopiclist;
                    $scope.getsubtopicdetails = promise.getsubtopicdetails;

                    $scope.mainunitdetails = [];

                    $scope.maintopicdetails = [];

                    $scope.topicdetails = [];

                    var unitindex = 0;
                    var maintopicindex = 0;

                    $scope.subjectname = "";
                    angular.forEach($scope.getsubjectdetails, function (sub) {
                        if (sub.ismS_Id === parseInt(obj.ismS_Id)) {
                            $scope.subjectname = sub.ismS_SubjectName;
                        }
                    });


                    if ($scope.getunitdetails !== null && $scope.getunitdetails.length > 0) {

                        if ($scope.gettopiclist !== null && $scope.gettopiclist.length > 0) {

                            if ($scope.getsubtopicdetails !== null && $scope.getsubtopicdetails.length > 0) {

                                angular.forEach($scope.getunitdetails, function (unit) {
                                    unitindex = unitindex + 1;

                                    $scope.maintopicdetails = [];

                                    angular.forEach($scope.gettopiclist, function (maintopic) {
                                        maintopicindex = maintopicindex + 1;

                                        $scope.topicdetails = [];
                                        if (unit.lpmU_Id === maintopic.lpmU_Id) {
                                            angular.forEach($scope.getsubtopicdetails, function (topic) {
                                                if (unit.lpmU_Id === maintopic.lpmU_Id && maintopic.lpmmT_Id === topic.lpmmT_Id) {
                                                    $scope.topicdetails.push({
                                                        LPMT_Id: topic.lpmT_Id, LPMT_TopicName: topic.lpmT_TopicName, maintopicindexlist: maintopicindex,
                                                        LPMT_LessonPlan: topic.lpmT_LessonPlan, LPMT_TopicOrder: topic.lpmT_TopicOrder, ISMS_Id: topic.ismS_Id
                                                    });
                                                }
                                            });
                                            $scope.maintopicdetails.push({
                                                LPMMT_Id: maintopic.lpmmT_Id, LPMMT_TopicName: maintopic.lpmmT_TopicName,
                                                LPMMT_TopicDescription: maintopic.lpmmT_TopicDescription, LPMMT_Order: maintopic.lpmmT_Order,
                                                unitindexlist: unitindex, topicdetailslist: $scope.topicdetails, maintopicindexlist: maintopicindex
                                            });
                                        }
                                    });
                                    $scope.mainunitdetails.push({
                                        LPMU_Id: unit.lpmU_Id, LPMU_UnitName: unit.lpmU_UnitName, unitindexlist: 'active:accordion == ' + unitindex,
                                        ISMS_Id: unit.ismS_Id, maintopicdetailslist: $scope.maintopicdetails
                                    });
                                });
                                console.log($scope.mainunitdetails);
                                $scope.btnshow = false;
                                $("#divsubjects").hide();

                            } else {
                                swal("Topic Details Not Found");
                            }
                        } else {
                            swal("Main Topic Details Not Found");
                        }
                    } else {
                        swal("Unit Details Not Found");
                    }
                } else {
                    swal("No Data Found");
                }
            });
        };

        $scope.getdocuments = function (obj) {
            $scope.getdocumentlistnew = [];
            $scope.uploaddocfiles = [];
            var data = obj;
            data.AMSE_Id = $scope.AMSE_Id

            apiService.create("LMSStudent/getcollegedocuments", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getdocumentlistnew = promise.getdocumentlist;
                    if ($scope.getdocumentlistnew !== null && $scope.getdocumentlistnew.length > 0) {
                        $scope.uploaddocfiles = $scope.getdocumentlistnew;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;

                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_Resources;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.oneAtATime = true;

        $scope.obj = {};

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.alldates).indexOf($scope.searchValue) >= 0;
        };

        var imagedownload = "";
        var docname = "";
        var studentreg = "";

        $scope.downloaddirectimage = function (data, idd) {
            studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
            console.log($scope.view_videos);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.onviewppt = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.contantppt = "";
            var fileURL = "";
            var file = "";
            $scope.contantppt = $sce.trustAsResourceUrl(filepath);
            $('#showppt').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        $scope.openNewBackgroundTab = function (data, name) {
            var a = document.createElement("a");
            a.href = data;
            var evt = document.createEvent("MouseEvents");
            evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            a.dispatchEvent(evt);
        };
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