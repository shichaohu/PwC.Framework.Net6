"use strict";
// 依赖GlobalUtil.js
var tool = window.tool || {};
(function () {

    // 是否是创建窗体
    this.isFormCreate = function () {
        // 获取当前 form 的状态：0:Undefined 1:Create 2:Update 3:Read Only 4:Disabled 6:Bulk Edit
        let fromStatus = Xrm.Page.ui.getFormType();
        return fromStatus == 1;
    }

    // 是否是更新窗体
    this.isFormUpdate = function () {
        // 获取当前 form 的状态：0:Undefined 1:Create 2:Update 3:Read Only 4:Disabled 6:Bulk Edit
        let fromStatus = Xrm.Page.ui.getFormType();
        return fromStatus == 2;
    }

    // 表单取值、赋值
    this.val = function (ctx, key, value) {
        let attr = ctx.getAttribute(key);
        if (!attr) return;
        if (value) {
            attr.setValue(value);
            return;
        }
        return attr.getValue();
    }

    /**
     * 字段为空则赋值
     * @param {object} ctx 执行上下问
     * @param {string} key 字段名
     * @param {any} val 值
     */
    this.setIfNull = function (ctx, key, val) {
        let attr = ctx.getAttribute(key);
        if (!attr) return;

        let value = attr.getValue(val);
        if (!value) {
            attr.setValue(val);
        }
    }

    // 获取关系ID
    this.erId = function (ctx, key) {
        let er = tool.val(ctx, key);
        let id = null;
        if (er) {
            id = er[0].id.replace("{", "").replace("}", "");
        }
        return id;
    }

    // 获取关系Name
    this.erName = function (ctx, key) {
        let er = tool.val(ctx, key);
        let name = null;
        if (er) {
            name = er[0].name;
        }
        return name;
    }

    // 获取选项的Text
    this.txt = function (ctx, key) {
        let attr = ctx.getAttribute(key);
        if (!attr) return "";

        return attr.getText();
    }

    // 清除字段
    this.clear = function (ctx, key) {
        ctx.getAttribute(key).setValue(null);
    }

    //设置必填项
    this.require = function (ctx, attrName, isRequired) {
        if (isRequired)
            ctx.getAttribute(attrName).setRequiredLevel("required");
        else
            ctx.getAttribute(attrName).setRequiredLevel("none");
    }

    // 隐藏字段
    this.hide = function (ctx, attrName) {
        ctx.getControl(attrName).setVisible(false);
    }

    // 显示字段
    this.show = function (ctx, attrName) {
        ctx.getControl(attrName).setVisible(true);
    }

    // 隐藏section
    this.hideSection = function (ctx, tabName, sectionName) {
        let tabObj = ctx.ui.tabs.get(tabName);
        if (tabObj) {
            let sectionObj = tabObj.sections.get(sectionName);
            if (sectionObj) {
                sectionObj.setVisible(false);
            }
        }
    }

    // 锁定字段
    this.lock = function (ctx, attr) {
        if (Array.isArray(attr)) {
            for (var i = 0; i < attr.length; i++) {
                let ctrl = ctx.getControl(attr[i]);
                if (ctrl) {
                    ctrl.setDisabled(true);
                }
            }
            return;
        }
        ctx.getControl(attr).setDisabled(true);
    }

    // 解锁字段
    this.unlock = function (ctx, attr) {
        if (Array.isArray(attr)) {
            for (var i = 0; i < attr.length; i++) {
                ctx.getControl(attr[i]).setDisabled(false);
            }
            return;
        }
        ctx.getControl(attr).setDisabled(false);
    }

    // 显示section
    this.showSection = function (ctx, tabName, sectionName) {
        let tabObj = ctx.ui.tabs.get(tabName);
        if (tabObj) {
            let sectionObj = tabObj.sections.get(sectionName);
            if (sectionObj) {
                sectionObj.setVisible(true);
            }
        }
    }

    // 隐藏tab
    this.hideTab = function (ctx, tabName) {
        let tabObj = ctx.ui.tabs.get(tabName);
        if (tabObj) {
            tabObj.setVisible(false);
        }
    }

    // 显示tab
    this.showTab = function (ctx, tabName) {
        let tabObj = ctx.ui.tabs.get(tabName);
        if (tabObj) {
            tabObj.setVisible(true);
        }
    }

    this.subLock = function (exeCtx, attrs) {
        var entityObj = exeCtx.getFormContext().data.entity;
        entityObj.attributes.forEach((attr, i) => {
            var ctrl = attr.controls.get(0);
            for (let item of attrs) {
                if (attr.getName() == item) {
                    ctrl.setDisabled(true);
                }
            }

        });
    }


    /**
     * 获取用户的角色
     * @param {object} formContext
     * @returns {array} 用户的角色
     */
    this.getUserRoles = function (ctx) {
        return ctx.context.userSettings.roles.getAll()
    }

    /**
     * 是否包含角色
     * @param {object} formContext
     * @param {string|array} role 角色名称
     * @returns {bool}
     */
    this.hasRole = function (ctx, role) {
        let roles = tool.getUserRoles(ctx);
        if (typeof role === "string") {
            for (let item of roles) {
                if (item.name == role) {
                    return true;
                }
            }
        }

        if (Array.isArray(role)) {
            for (let i = 0; i < role.length; i++) {
                let roleName = role[i];

                for (let item of roles) {
                    if (item.name == roleName) {
                        return true;
                    }
                }
            }
        }

        return false;
    }





    // 按钮引用
    this.loadTool = function () { }

    // get 同步请求
    this.get = function (logicalName, xml, format) {
        let collectionName = tool.pluralize(logicalName);
        let headers = [];
        if (format === true) {
            headers.push({
                name: "Prefer",
                value: "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\""
            })
        }
        var result = GlobalUtil.WebApi.Sync.get(`/${collectionName}?fetchXml=${encodeURIComponent(xml)}`, headers);
        if (result.success) {
            return result.data.value;
        }
        console.error(`get 同步请求 失败:${logicalName},`, result);
        return null;
    }

    // get 同步请求
    this.getSync = function (xml, format) {
        let logicalName = tool.getEntityName(xml);
        return tool.get(logicalName, xml, format);
    }

    // get 同步请求
    /**
     * get 同步请求,获取单个实体
     * @param {any} logicalName 实体名称
     * @param {any} xml fetchxml
     * @param {any} format 是否获取查找字段的Name属性,默认false
     * @returns 实体信息
     */
    this.getOne = function (logicalName, xml, format) {
        let result = tool.get(logicalName, xml, format);
        if (result && result.length > 0) {
            return result[0];
        }
        return null;
    }


    // 统计数量
    this.count = function (logicalName, xml) {
        var result = tool.get(logicalName, xml);
        if (result && result.length > 0) {
            let count = result[0].count;
            return count;
        }
    }

    // 单词转复数形式
    this.pluralize = function (word) {
        const specials = {
            'ox': 'oxen',
            'child': 'children',
            'foot': 'feet',
            'tooth': 'teeth',
            'man': 'men',
            'woman': 'women',
            'person': 'people',
            'mouse': 'mice',
            'goose': 'geese',
            'deer': 'deer',
            'fish': 'fish',
            'sheep': 'sheep',
            'species': 'species',
            'aircraft': 'aircraft',
            'watercraft': 'watercraft',
            'spacecraft': 'spacecraft',
            'hovercraft': 'hovercraft'
        }

        const irregulars = {
            'beef': 'beefs',
            'brother': 'brothers',
            'cow': 'cows',
            'genie': 'genies',
            'genus': 'genera',
            'index': 'indices',
            'money': 'monies',
            'octopus': 'octopuses',
            'opus': 'opuses',
            'ox': 'oxen',
            'soliloquy': 'soliloquies',
            'testis': 'testes',
            'thesis': 'theses'
        }

        const ending = word.slice(-2);
        const last = word.slice(-1);
        const vowels = ['a', 'e', 'i', 'o', 'u'];
        const consonants = 'bcdfghjklmnpqrstvwxyz';

        // 检查特殊情况
        if (specials.hasOwnProperty(word)) {
            return specials[word];
        }

        // 检查不规则情况
        if (irregulars.hasOwnProperty(word)) {
            return irregulars[word];
        }

        // 如果单词以辅音字母 + y 结尾，则将 y 替换为 i，再加 es
        if (consonants.includes(word.slice(-2, -1)) && last === 'y') {
            return word.slice(0, -1) + 'ies';
        }

        // 如果单词以元音字母 + y 结尾，则在末尾添加 s
        if (vowels.includes(word.slice(-2, -1)) && last === 'y') {
            return word + 's';
        }

        // 如果单词以辅音字母 + o 结尾，则在结尾处添加 es
        if (consonants.includes(word.slice(-2, -1)) && last === 'o') {
            return word + 'es';
        }

        // 如果单词以元音字母 + o 结尾，则在末尾添加 s 或者添加 es
        if (vowels.includes(word.slice(-2, -1)) && last === 'o') {
            return word + 's';
        }

        // 如果单词以 f 或者 fe 结尾，则将 f 或者 fe 替换为 ves
        if (last === 'f') {
            return word.slice(0, -1) + 'ves';
        }

        if (word.slice(-2) === 'fe') {
            return word.slice(0, -2) + 'ves';
        }

        // 如果单词已经以 s, x, z, ch, sh 结尾，则在结尾处添加 es
        if (ending === 'ch' || ending === 'sh' ||
            ending === 'ss' || ending === 'zz' ||
            last === 'x' || last === 's') {
            return word + 'es';
        }

        // 其他情况在末尾添加 s
        return word + 's';

    }

    // 从fetchXml获取实体名称
    this.getEntityName = function (fetchXml) {
        let entityName = "";

        const pattern = /<entity name=['"](.*?)['"]/;
        const matchResult = fetchXml.match(pattern);
        if (matchResult && matchResult.length >= 2) {
            entityName = matchResult[1];
        } else {
            console.error(`实体名称无法解析:${fetchXml}`);
        }

        return entityName;
    }

}).call(tool);