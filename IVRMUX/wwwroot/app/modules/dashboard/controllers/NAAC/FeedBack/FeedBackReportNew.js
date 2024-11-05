(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedBackReportNewController', FeedBackReportNewController)

    FeedBackReportNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedBackReportNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#30859c",
                "#ff8c00",
                "#a9a9a9",
                "#cccc00",
                "#9dd2e1",
                "#EDCA93",
                "#696661",
                "#695A42",
                "#B6B1A8"
            ]);

        $scope.semlist = [
            { semid: 1, semyear: "1 Year" },
            { semid: 2, semyear: "2 Year" },
            { semid: 3, semyear: "3 Year" }
        ];

        $scope.report1 = [];
        $scope.imgdiv = false;
        $scope.imgdiv1 = false;
        $scope.report = false;
        $scope.catreport = false;
        $scope.table = false;
        $scope.graphs = false;
        $scope.printSectionIdoverall = false;
        $scope.printSectionIdyearwise = false;
        $scope.printSectionIdquestion = false;
        $scope.printSectionIdquestionpie = false;
        $scope.printSectionIdtotal = false;
        $scope.printSectionIdremarks = false;


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;

        var chart = {};

        $scope.BindData = function () {
            apiService.getDATA("FeedBackReport/getdetails").
                then(function (promise) {
                    $scope.yearlist = promise.getyear;
                });
        };

        $scope.onchangeradio = function () {
            $scope.printSectionIdoverall = false;
            $scope.printSectionIdyearwise = false;
            $scope.printSectionIdquestion = false;
            $scope.printSectionIdquestionpie = false;
            $scope.printSectionIdtotal = false;
            $scope.printSectionIdremarks = false;
            $scope.report1 = [];
            $scope.table = false;
            $scope.graphs = false;

            var data = {
                "flag": $scope.FMTY_StakeHolderFlag
            };
            apiService.create("FeedBackReport/onchangeradio", data).then(function (promise) {

                if (promise !== null) {

                    $scope.feedbacktype1 = promise.feedbacktype;
                    if ($scope.feedbacktype1.length > 0) {
                        $scope.feedbacktype = promise.feedbacktype;

                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangeyear = function () {
            $scope.table = false;
            $scope.graphs = false;
            $scope.printSectionIdoverall = false;
            $scope.printSectionIdyearwise = false;
            $scope.printSectionIdquestion = false;
            $scope.printSectionIdquestionpie = false;
            $scope.printSectionIdremarks = false;
            $scope.printSectionIdtotal = false;
            $scope.report1 = [];
            $scope.AMCO_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("FeedBackReport/onchangeyear", data).then(function (promise) {

                if (promise !== null) {

                    $scope.getcourse1 = promise.getcourse;
                    if ($scope.getcourse1.length > 0) {
                        $scope.coursedetails = promise.getcourse;

                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangeradioflag = function () {
            $scope.printSectionIdoverall = false;
            $scope.printSectionIdyearwise = false;
            $scope.printSectionIdquestion = false;
            $scope.printSectionIdquestionpie = false;
            $scope.printSectionIdremarks = false;
            $scope.printSectionIdtotal = false;
            $scope.table = false;
            $scope.graphs = false;
            $scope.FMTY_Id = "";
            $scope.FMQE_Id = "";
            $scope.AMCO_Id = "";
            $scope.report1 = [];
        };

        $scope.onchangepiecolumnflag = function () {
            $scope.printSectionIdoverall = false;
            $scope.printSectionIdyearwise = false;
            $scope.printSectionIdquestion = false;
            $scope.printSectionIdquestionpie = false;
            $scope.printSectionIdremarks = false;
            $scope.printSectionIdtotal = false;
            $scope.table = false;
            $scope.graphs = false;
            $scope.report1 = [];
        };


        $scope.onchangefeedback = function () {
            $scope.printSectionIdoverall = false;
            $scope.printSectionIdyearwise = false;
            $scope.printSectionIdquestion = false;
            $scope.printSectionIdquestionpie = false;
            $scope.printSectionIdremarks = false;
            $scope.printSectionIdtotal = false;
            $scope.table = false;
            $scope.graphs = false;
            $scope.report1 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "flag": $scope.FMTY_StakeHolderFlag,
                "Type": $scope.FMTY_Id,
                "Flagtype": $scope.flagtype
            };

            apiService.create("FeedBackReport/onchangefeedback", data).then(function (promise) {

                if (promise !== null) {

                    $scope.questiondetails = promise.getquestions;

                } else {
                    swal("Something Went Wrong Contact Administrator");
                }

            });
        };

        $scope.searchValue = '';
        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        // For table represtation
        $scope.onreport = function (obj) {
            $scope.report1 = [];
            if ($scope.myForm.$valid) {
                $scope.table = false;
                $scope.graphs = false;

                var qestionid = 0;
                var courseif = 0;
                var report1 = 0;
                if ($scope.flagtype !== 'question' && $scope.flagtype !== 'total' && $scope.flagtype !== 'remarks') {
                    qestionid = 0;
                    courseif = $scope.AMCO_Id;
                } else if ($scope.flagtype === 'question') {
                    qestionid = $scope.FMQE_Id;
                    courseif = 0;
                } else if ($scope.flagtype === 'remarks') {
                    qestionid = $scope.FMQE_Id;
                    courseif = $scope.AMCO_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMTY_StakeHolderFlag": $scope.FMTY_StakeHolderFlag,
                    "FMTY_Id": $scope.FMTY_Id,
                    "AMCO_Id": courseif,
                    "Flagtype": $scope.flagtype,
                    "FMQE_Id": qestionid,
                    "reportnew": report1,
                    "graphtype": $scope.piecolumn
                };

                apiService.create("FeedBackReport/getreportnew", data).
                    then(function (promise) {

                        if (promise !== null) {
                            $scope.report1 = promise.getreportdetails;

                            $scope.reporttype = $scope.FMTY_StakeHolderFlag;

                            angular.forEach($scope.yearlist, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = dd.asmaY_Year;
                                    $scope.yearnamedetails = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.feedbacktype, function (dddd) {
                                if (dddd.fmtY_Id === parseInt($scope.FMTY_Id)) {
                                    $scope.type = dddd.fmtY_FeedbackTypeName;
                                    $scope.questionflag = dddd.fmtY_QuestionwiseOptionFlg;
                                }
                            });


                            if ($scope.report1.length > 0) {

                                $scope.table = true;
                                $scope.graphs = false;

                                $scope.report = promise.getreportdetails;
                                $scope.temp123 = [];
                                $scope.temp123new = [];

                                $scope.report = promise.getreportdetails;

                                $scope.getquestions = promise.getquestions;
                                $scope.getoptions = promise.getoptions;

                                $scope.getsemester = [];

                                if ($scope.flagtype !== 'question') {
                                    angular.forEach($scope.coursedetails, function (d) {
                                        if (d.amcO_Id === parseInt($scope.AMCO_Id)) {
                                            $scope.coursename = d.amcO_CourseName;
                                            $scope.courseid = d.amcO_Id;
                                        }
                                    });
                                }
                                $scope.reportname = $scope.type;

                                $scope.yearname = "Year :" + $scope.yearname;

                                if ($scope.flagtype === 'Yearwise') {
                                    $scope.printSectionIdoverall = false;
                                    $scope.printSectionIdyearwise = true;
                                    $scope.printSectionIdquestion = false;
                                    $scope.printSectionIdquestionpie = false;
                                    $scope.printSectionIdtotal = false;
                                    $scope.printSectionIdremarks = false;
                                    $scope.reportname = "Semester Wise " + $scope.type;
                                    angular.forEach($scope.report, function (dd) {
                                        if ($scope.getsemester.length === 0) {
                                            $scope.getsemester.push({ AMSE_Id: dd.amseid, semyear: dd.semnameyear });
                                        } else if ($scope.getsemester.length > 0) {
                                            var count = 0;
                                            angular.forEach($scope.getsemester, function (ddd) {
                                                if (parseInt(ddd.AMSE_Id) === parseInt(dd.amseid)) {
                                                    count += 1;
                                                }
                                            });

                                            if (count === 0) {
                                                $scope.getsemester.push({ AMSE_Id: dd.amseid, semyear: dd.semnameyear });
                                            }
                                        }
                                    });
                                    console.log($scope.getsemester);

                                    $scope.tempnew = [];

                                    angular.forEach($scope.getsemester, function (de) {
                                        $scope.tempnew.push({
                                            coursenamedetails: $scope.coursename, coursedetailsid: $scope.courseid, semid: de.AMSE_Id, semyearname: de.semyear,
                                            rowcount: $scope.getquestions.length
                                        });
                                    });
                                    console.log($scope.tempnew);
                                }

                                else if ($scope.flagtype === 'Overall') {
                                    $scope.printSectionIdoverall = true;
                                    $scope.printSectionIdyearwise = false;
                                    $scope.printSectionIdquestion = false;
                                    $scope.printSectionIdquestionpie = false;
                                    $scope.printSectionIdtotal = false;
                                    $scope.printSectionIdremarks = false;
                                    $scope.tempnew = [];
                                    $scope.tempnew.push({
                                        coursenamedetails: $scope.coursename, coursedetailsid: $scope.courseid, rowcount: $scope.getquestions.length
                                    });


                                     // OPTIONS WISE QUESTIONS
                                    if ($scope.questionflag !== true) {
                                        angular.forEach($scope.getoptions, function (dd) {
                                            $scope.tempoption = [];
                                            angular.forEach($scope.report, function (ddd) {
                                                if (dd.fmoP_Id === ddd.opid) {
                                                    angular.forEach($scope.getquestions, function (dddd) {
                                                        if (dddd.fmqE_Id === ddd.qid) {
                                                            $scope.tempoption.push({ questionname: dddd.fmqE_FeedbackQRemarks, optionscore: ddd.total });
                                                        }
                                                    });
                                                }
                                            });
                                            $scope.temp123new.push({ header: dd.fmoP_FeedbackOptions, optiondetails: $scope.tempoption });
                                        });

                                    }
                                    else {
                                        angular.forEach($scope.getoptions, function (dd) {
                                            $scope.tempoption = [];
                                            angular.forEach($scope.report, function (ddd) {
                                                if (dd.fmoP_Id === ddd.opid && dd.fmqE_Id === ddd.qid) {
                                                    angular.forEach($scope.getquestions, function (dddd) {
                                                        if (dddd.fmqE_Id === ddd.qid) {
                                                            $scope.tempoption.push({ questionname: dddd.fmqE_FeedbackQRemarks, optionscore: ddd.total });
                                                        }
                                                    });
                                                }
                                            });
                                            $scope.temp123new.push({ header: dd.fmoP_FeedbackOptions, optiondetails: $scope.tempoption });
                                        });
                                    }

                                    console.log("---------------");
                                    console.log($scope.temp123new);


                                    var datagraph = [];
                                    var datagraphpints = [];

                                    $scope.stdgraphseries3 = [];
                                    $scope.stdgraphseries4 = [];
                                    angular.forEach($scope.temp123new, function (gr) {
                                        datagraphpints = [];
                                        $scope.stdgraphseries3 = [];

                                        for (var i = 0; i < gr.optiondetails.length; i++) {
                                            $scope.stdgraphseries3.push({ label: gr.optiondetails[i].questionname, "y": gr.optiondetails[i].optionscore });
                                        }
                                        console.log($scope.stdgraphseries3);

                                        $scope.stdgraphseries4.push({
                                            type: 'stackedColumn',
                                            name: gr.header,
                                            showInLegend: true,
                                            indexLabel: "{y}",
                                            indexLabelFontColor: "black",
                                            indexLabelFontSize: 14,
                                            dataPoints: $scope.stdgraphseries3
                                        });
                                    });


                                    angular.forEach($scope.coursedetails, function (cc) {

                                        if (parseInt(cc.amcO_Id) === parseInt($scope.AMCO_Id)) {
                                            $scope.coursenamenew = cc.amcO_CourseName;
                                        }
                                    });


                                    chart = new CanvasJS.Chart("chartContainer", {
                                        animationEnabled: true,
                                        height: 500,
                                        width: 1070,
                                        title: {
                                            text: $scope.coursenamenew + ' ' + $scope.reportname
                                        },
                                        colorSet: "graphcolor",
                                        dataPointWidth: 40,
                                        axisX: {
                                            interval: 1,
                                            title: "Key Points Questions",
                                            labelFontSize: 12,
                                            labelFontWeight: "bold",
                                            labelFontColor: "black"
                                        },
                                        axisY: {
                                            title: "Percentage",
                                            labelFontSize: 12,
                                            labelFontWeight: "bold",
                                            labelFontColor: "black",
                                            gridThickness: 0
                                        },
                                        toolTip: {
                                            shared: true
                                        },
                                        legend: {
                                            reversed: true
                                        },
                                        data: $scope.stdgraphseries4
                                    });
                                    chart.render();
                                }

                                else if ($scope.flagtype === 'question') {
                                    var columnname = "";
                                    if ($scope.piecolumn === 'columnper') {
                                        columnname = 'Percentage';
                                        $scope.printSectionIdquestion = true;
                                        $scope.printSectionIdquestionpie = false;
                                    } else if ($scope.piecolumn === 'columnno') {
                                        columnname = 'Number';
                                        $scope.printSectionIdquestion = true;
                                        $scope.printSectionIdquestionpie = false;
                                    } else if ($scope.piecolumn === 'pie') {
                                        $scope.printSectionIdquestion = false;
                                        $scope.printSectionIdquestionpie = true;
                                    }

                                    $scope.getcoursenew = promise.getcoursenew;
                                    $scope.printSectionIdoverall = false;
                                    $scope.printSectionIdyearwise = false;
                                    $scope.printSectionIdtotal = false;
                                    $scope.printSectionIdremarks = false;
                                    $scope.reportname = "";

                                    angular.forEach($scope.questiondetails, function (ques) {
                                        if (ques.fmqE_Id === parseInt($scope.FMQE_Id)) {
                                            $scope.reportname = ques.fmqE_FeedbackQuestions;
                                        }
                                    });

                                    if ($scope.piecolumn !== 'pie') {

                                        $scope.getcoursenewdetails = [];
                                        angular.forEach($scope.report, function (cou) {
                                            if ($scope.getcoursenewdetails.length === 0) {
                                                $scope.getcoursenewdetails.push({ courseid: cou.amcoid, courseidname: cou.coursename });
                                            }
                                            else if ($scope.getcoursenewdetails.length > 0) {
                                                var count = 0;
                                                angular.forEach($scope.getcoursenewdetails, function (cc) {
                                                    if (parseInt(cc.courseid) === parseInt(cou.amcoid)) {
                                                        count += 1;
                                                    }
                                                });
                                                if (count === 0) {
                                                    $scope.getcoursenewdetails.push({ courseid: cou.amcoid, courseidname: cou.coursename });
                                                }
                                            }
                                        });

                                        $scope.temp123new = [];
                                        angular.forEach($scope.getoptions, function (dd) {
                                            $scope.tempoption = [];
                                            angular.forEach($scope.report, function (ddd) {
                                                if (dd.fmoP_Id === ddd.opid) {
                                                    angular.forEach($scope.getcoursenew, function (dddd) {
                                                        if (dddd.amcO_Id === ddd.amcoid) {
                                                            $scope.tempoption.push({ questionname: dddd.amcO_CourseName, optionscore: ddd.total });
                                                        }
                                                    });
                                                }
                                            });
                                            $scope.temp123new.push({ header: dd.fmoP_FeedbackOptions, optiondetails: $scope.tempoption });
                                        });

                                        console.log("---------------");
                                        console.log($scope.temp123new);


                                        $scope.stdgraphseries3 = [];
                                        $scope.stdgraphseries4 = [];
                                        angular.forEach($scope.temp123new, function (gr) {

                                            $scope.stdgraphseries3 = [];

                                            for (var i = 0; i < gr.optiondetails.length; i++) {
                                                $scope.stdgraphseries3.push({ label: gr.optiondetails[i].questionname, "y": gr.optiondetails[i].optionscore });
                                            }
                                            console.log($scope.stdgraphseries3);

                                            $scope.stdgraphseries4.push({
                                                type: 'stackedColumn',
                                                name: gr.header,
                                                showInLegend: true,
                                                indexLabel: "{y}",
                                                indexLabelFontColor: "black",
                                                indexLabelFontSize: 14,
                                                dataPoints: $scope.stdgraphseries3
                                            });
                                        });

                                        console.log($scope.stdgraphseries4);

                                        chart = {};
                                    }

                                    if ($scope.piecolumn === 'columnper') {
                                        chart = new CanvasJS.Chart("chartContainer1", {
                                            animationEnabled: true,
                                            height: 500,
                                            width: 1070,
                                            title: {
                                                text: $scope.reportname
                                            },
                                            colorSet: "graphcolor",
                                            dataPointWidth: 40,
                                            axisX: {
                                                interval: 1,
                                                title: "Courses",
                                                labelFontSize: 12,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black"
                                            },
                                            axisY: {
                                                title: columnname,
                                                labelFontSize: 12,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black",
                                                gridThickness: 0
                                            },
                                            toolTip: {
                                                shared: true
                                            },
                                            legend: {
                                                reversed: true
                                            },
                                            data: $scope.stdgraphseries4
                                        });
                                    }
                                    else if ($scope.piecolumn === 'columnno') {
                                        chart = new CanvasJS.Chart("chartContainer112", {
                                            animationEnabled: true,
                                            height: 500,
                                            width: 1070,
                                            title: {
                                                text: $scope.reportname
                                            },
                                            colorSet: "graphcolor",
                                            dataPointWidth: 40,
                                            axisX: {
                                                interval: 1,
                                                title: "Courses",
                                                labelFontSize: 12,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black"
                                            },
                                            axisY: {
                                                title: columnname,
                                                labelFontSize: 12,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black",
                                                gridThickness: 0
                                            },
                                            toolTip: {
                                                shared: true
                                            },
                                            legend: {
                                                reversed: true
                                            },
                                            data: $scope.stdgraphseries4
                                        });
                                    } else if ($scope.piecolumn === 'pie') {
                                        $scope.stdgraphseries4 = [];
                                        angular.forEach($scope.report, function (dd) {
                                            $scope.stdgraphseries4.push({ label: dd.options, "y": dd.total });
                                        });

                                        chart = new CanvasJS.Chart("chartContainer1pie", {
                                            animationEnabled: true,
                                            height: 500,
                                            width: 1070,
                                            title: {
                                                text: $scope.reportname
                                            },
                                            colorSet: "graphcolor",
                                            data: [{
                                                type: "pie",
                                                startAngle: 25,
                                                toolTipContent: "<b>{label}</b>: {y}%",
                                                showInLegend: "true",
                                                legendText: "{label}",
                                                indexLabelFontSize: 16,
                                                indexLabel: "{label} - {y}%",
                                                dataPoints: $scope.stdgraphseries4
                                            }]
                                        });
                                    }
                                    chart.render();
                                }

                                else if ($scope.flagtype === 'total') {
                                    $scope.printSectionIdoverall = false;
                                    $scope.printSectionIdyearwise = false;
                                    $scope.printSectionIdquestion = false;
                                    $scope.printSectionIdtotal = true;
                                    $scope.printSectionIdremarks = false;
                                    $scope.report = [];
                                    $scope.report = promise.getreportdetails;

                                    $scope.coursenewdetails = [];

                                    angular.forEach($scope.report, function (cou) {
                                        if ($scope.coursenewdetails.length === 0) {
                                            $scope.coursenewdetails.push({ courseid: cou.AMCO_Id, courseidname: cou.AMCO_CourseName });
                                        }
                                        else if ($scope.coursenewdetails.length > 0) {
                                            var count = 0;
                                            angular.forEach($scope.coursenewdetails, function (cc) {
                                                if (parseInt(cc.courseid) === parseInt(cou.AMCO_Id)) {
                                                    count += 1;
                                                }
                                            });
                                            if (count === 0) {
                                                $scope.coursenewdetails.push({ courseid: cou.AMCO_Id, courseidname: cou.AMCO_CourseName });
                                            }
                                        }
                                    });

                                    console.log("*************");
                                    console.log($scope.coursenewdetails);
                                    console.log("##############");
                                    console.log($scope.report);

                                    angular.forEach($scope.coursenewdetails, function (coud) {
                                        var total = 0;
                                        angular.forEach($scope.semlist, function (sem) {
                                            angular.forEach($scope.report, function (re) {
                                                if (parseInt(coud.courseid) === parseInt(re.AMCO_Id) && re.semyear === sem.semyear) {
                                                    total += re.GIVENSTUDENTS;
                                                }
                                            });
                                        });
                                        coud.total = total;
                                    });
                                }

                                else if ($scope.flagtype === "remarks") {

                                    $scope.printSectionIdoverall = false;
                                    $scope.printSectionIdyearwise = false;
                                    $scope.printSectionIdquestion = false;
                                    $scope.printSectionIdtotal = false;
                                    $scope.printSectionIdremarks = true;

                                    $scope.getreportdetails = promise.getreportdetails;

                                    if ($scope.getreportdetails !== null && $scope.getreportdetails.length > 0) {
                                        $scope.semlistnew = [];
                                        var countsem = 0;
                                        angular.forEach($scope.getreportdetails, function (d) {
                                            if ($scope.semlistnew.length === 0) {
                                                $scope.semlistnew.push({ AMSE_Id: d.AMSE_Id, AMSE_SEMName: d.AMSE_SEMName, ASMAY_Year: d.ASMAY_Year, AMCO_CourseName: d.AMCO_CourseName, FMQE_FeedbackQRemarks: d.FMQE_FeedbackQRemarks });
                                            } else if ($scope.semlistnew.length > 0) {

                                                angular.forEach($scope.semlistnew, function (dd) {
                                                    countsem = 0;
                                                    if (parseInt(dd.AMSE_Id) === parseInt(d.AMSE_Id)) {
                                                        countsem += 1;
                                                    }
                                                });

                                                if (countsem === 0) {
                                                    $scope.semlistnew.push({ AMSE_Id: d.AMSE_Id, AMSE_SEMName: d.AMSE_SEMName, ASMAY_Year: d.ASMAY_Year, AMCO_CourseName: d.AMCO_CourseName, FMQE_FeedbackQRemarks: d.FMQE_FeedbackQRemarks });
                                                }
                                            }
                                        });


                                        $scope.reportnewremarks = [];


                                        angular.forEach($scope.semlistnew, function (se) {
                                            $scope.tempremarksdetails = [];
                                            angular.forEach($scope.getreportdetails, function (ser) {
                                                if (parseInt(se.AMSE_Id) === parseInt(ser.AMSE_Id)) {
                                                    $scope.tempremarksdetails.push({ FMQE_Id: ser.FMQE_Id, FCSTR_FeedBack: ser.FCSTR_FeedBack });
                                                }
                                            });

                                            $scope.reportnewremarks.push({
                                                AMSE_Id: se.AMSE_Id, AMSE_SEMName: se.AMSE_SEMName, ASMAY_Year: se.ASMAY_Year, AMCO_CourseName: se.AMCO_CourseName,
                                                FMQE_FeedbackQRemarks: se.FMQE_FeedbackQRemarks,
                                                details: $scope.tempremarksdetails, rowcount: $scope.tempremarksdetails.length
                                            });
                                        });

                                        console.log($scope.reportnewremarks);
                                    } else {
                                        swal("No Records Found");
                                        $scope.table = false;
                                        $scope.graphs = false;
                                    }
                                }

                            } else {
                                swal("No Records Found");
                                $scope.table = false;
                                $scope.graphs = false;
                            }

                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                            $scope.table = false;
                            $scope.graphs = false;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        // View Details
        $scope.getstudentlistgiven = function (obj) {
            $scope.search55 = "";
            var qestionid = 0;
            var courseif = 0;
            var report1 = 0;
            if ($scope.flagtype !== 'question' && $scope.flagtype !== 'total') {
                qestionid = 0;
                courseif = $scope.AMCO_Id;
            } else if ($scope.flagtype === 'question') {
                qestionid = $scope.FMQE_Id;
                courseif = 0;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMTY_StakeHolderFlag": $scope.FMTY_StakeHolderFlag,
                "FMTY_Id": $scope.FMTY_Id,
                "AMCO_Id": obj.courseid,
                "Flagtype": $scope.flagtype,
                "FMQE_Id": qestionid,
                "reportnew": 1
            };

            apiService.create("FeedBackReport/getstudentlist", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getstudentlist = promise.getstudentlist;
                    $scope.feedback = "Feedback Given ";
                } else {
                    swal("No Records Found");
                }

            });

        };

        $scope.getstudentlistnotgiven = function (obj) {
            $scope.search55 = "";
            var qestionid = 0;
            var courseif = 0;
            var report1 = 0;
            if ($scope.flagtype !== 'question' && $scope.flagtype !== 'total') {
                qestionid = 0;
                courseif = $scope.AMCO_Id;
            } else if ($scope.flagtype === 'question') {
                qestionid = $scope.FMQE_Id;
                courseif = 0;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMTY_StakeHolderFlag": $scope.FMTY_StakeHolderFlag,
                "FMTY_Id": $scope.FMTY_Id,
                "AMCO_Id": obj.courseid,
                "Flagtype": $scope.flagtype,
                "FMQE_Id": qestionid,
                "reportnew": 2
            };

            apiService.create("FeedBackReport/getstudentlist", data).then(function (promise) {
                if (promise !== null) {
                    $scope.feedback = "Feedback Not Given ";
                    $scope.getstudentlist = promise.getstudentlist;
                } else {
                    swal("No Records Found");
                }

            });

        };

        $scope.PrintChart = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.flagtype === 'Yearwise') {
                innerContents = document.getElementById("printSectionIdyearwise").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            } else if ($scope.flagtype === 'Overall') {
                var base64Image = chart.canvas.toDataURL();
                document.getElementById('chartContainer').style.display = 'none';
                document.getElementById('chartImage').src = base64Image;
                innerContents = document.getElementById("printSectionIdoverall").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                $scope.imgdiv = true;
                $scope.imgdiv1 = false;
                $scope.imgdiv112 = false;
                $scope.imgdiv1pie = false;
                popupWinindow.document.close();
            } else if ($scope.flagtype === 'question') {
                var base64Image1 = "";
                if ($scope.piecolumn === 'columnper') {
                    base64Image1 = chart.canvas.toDataURL();
                    document.getElementById('chartContainer1').style.display = 'none';
                    document.getElementById('chartImage1').src = base64Image1;
                    innerContents = document.getElementById("printSectionIdquestion").innerHTML;
                    popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                    $scope.imgdiv = false;
                    $scope.imgdiv112 = false;
                    $scope.imgdiv1 = true;
                    $scope.imgdiv1pie = false;
                } else if ($scope.piecolumn === 'columnno') {
                    base64Image1 = chart.canvas.toDataURL();
                    document.getElementById('chartContainer112').style.display = 'none';
                    document.getElementById('chartImage112').src = base64Image1;
                    innerContents = document.getElementById("printSectionIdquestion").innerHTML;
                    popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                    $scope.imgdiv = false;
                    $scope.imgdiv1 = false;
                    $scope.imgdiv112 = true;
                    $scope.imgdiv1pie = false;
                } else if ($scope.piecolumn === 'pie') {
                    base64Image1 = chart.canvas.toDataURL();
                    document.getElementById('chartContainer1pie').style.display = 'none';
                    document.getElementById('chartImage1pie').src = base64Image1;
                    innerContents = document.getElementById("printSectionIdquestionpie").innerHTML;
                    popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                        '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                        '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                        '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                    $scope.imgdiv = false;
                    $scope.imgdiv1 = false;
                    $scope.imgdiv112 = false;
                    $scope.imgdiv1pie = true;
                }
                popupWinindow.document.close();

            } else if ($scope.flagtype === 'total') {
                innerContents = document.getElementById("printSectionIdtotalnew").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.flagtype === 'remarks') {
                innerContents = document.getElementById("printSectionIdremarks").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

        };

        $scope.printstudentlist = function () {
            var innerContents = "";
            var popupWinindow = "";
            innerContents = document.getElementById("printSectionIdtotalstudentlist").innerHTML;
            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '<link type="text/css" media="print" href="css/print/NaacReportFeedback/FeedbackReportPdf.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();


        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }

})();