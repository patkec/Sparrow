module sparrow.viewModels {
    'use strict';

    // ViewModels which enable user to see calculated values while editing.

    export class DraftViewModel {
        private _editDiscount: number;        
        private _items: DraftItemViewModel[];
        private _newItem: DraftItemViewModel;

        id: any;
        title: string;
        customer: any;
        discount: number;
        subtotal: number;
        total: number;
        discountAmount: number;

        get isDirty(): boolean {
            return this._editDiscount || $.grep(this._items, function (el) { return el.isDirty; }).length > 0;
        }

        get items(): DraftItemViewModel[]{
            return this._items;
        }

        set items(value: DraftItemViewModel[]) {
            this._items = [];
            if (value) {
                for (var i = 0; i < value.length; i++) {
                    if (typeof value[i] === typeof DraftItemViewModel)
                        this._items.push(value[i]);
                    // Wrap in view model if necessary
                    else
                        this._items.push(new DraftItemViewModel(value[i]));
                }   
            }
        }

        get newItem(): DraftItemViewModel {
            return this._newItem;
        }

        constructor(source: any) {
            console.log(source);
            this._items = [];
            this.discount = 0;
            // Copy all the properties from source to current view model
            for (var key in source)
                if (key[0] !== '$')
                    this[key] = source[key];
        }

        addNewItem() {
            this._newItem = new DraftItemViewModel({});
            this._items.push(this._newItem);
        }

        updateEditDiscount(value: number) {
            this._editDiscount = value;
        }

        endEdit() {
            delete this._editDiscount;
        }

        calcSubtotal() {
            var sum = this.subtotal;
            // Use client-side calculated subtotal only when editing.
            if (this.isDirty) {
                sum = 0;
                $.each(this._items, function (i, item) {
                    sum = sum + item.calcTotal();
                });
            }
            return sum;
        }

        calcTotal() {
            // Use client-side calculated total only when editing.
            var total = !this.isDirty && this.total;
            return total || (this.calcSubtotal() * (1 - (this._editDiscount || this.discount) / 100));
        }

        calcDiscountAmount() {
            // Use client-side calculated discount amount only when editing.
            var discountAmount = !this.isDirty && this.discountAmount;
            return discountAmount || (this.calcSubtotal() * (this._editDiscount || this.discount) / 100);
        }
    }

    export class DraftItemViewModel {
        private _isDirty: boolean;
        private _editInfo: any;
        private _subtotal: number;
        private _total: number;

        id: any;
        quantity: number;
        discount: number;
        product: any;

        get isDirty(): boolean {
            return this._isDirty;
        }

        constructor(source: any) {            
            this.quantity = 0;
            this.discount = 0;
            this._isDirty = false;
            // Copy all the properties from source to current view model
            for (var key in source)
                if (key[0] !== '$')
                    this[key] = source[key];
        }

        beginEdit() {
            this._editInfo = {};
            this.updateEditProduct(this.product);
            this.updateEditQuantity(this.quantity);
            this.updateEditDiscount(this.discount);
            this._isDirty = false;
        }

        endEdit(data) {
            this._isDirty = false;
            delete this._editInfo;
            if (data) {
                this._total = data.total;
                this._subtotal = data.subtotal;
            }
        }

        updateEditProduct(product: any) {
            this._isDirty = true;
            this._editInfo.price = product ? product.price : 0;
        }

        updateEditQuantity(quantity: number) {
            if (this._editInfo.quantity !== quantity) {
                this._isDirty = true;
                this._editInfo.quantity = quantity;
            }            
        }

        updateEditDiscount(discount: number) {
            if (this._editInfo.discount !== discount) {
                this._isDirty = true;
                this._editInfo.discount = discount;
            }            
        }

        getQuantity() {
            if (this._editInfo)
                return this._editInfo.quantity;
            return this.quantity;
        }

        getDiscount() {
            if (this._editInfo)
                return this._editInfo.discount;
            return this.discount;
        }

        getPrice() {
            if (this._editInfo)
                return this._editInfo.price;
            return this.product ? this.product.price : 0;
        }

        calcSubtotal() {
            // Use client-side calculated subtotal only when editing.
            var subtotal = !this._isDirty && this._subtotal;
            return subtotal || (this.getPrice() * this.getQuantity());
        }

        calcTotal() {
            // Use client-side calculated total only when editing.
            var total = !this._isDirty && this._total;
            return total || (this.calcSubtotal() * (1 - this.getDiscount() / 100));
        }
    }
}