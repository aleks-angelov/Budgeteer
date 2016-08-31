export class TransactionViewModel {
    constructor(
        public date: Date,
        public amount: number,
        public personName: string,
        public categoryName: string,
        public isDebit: boolean,
        public note?: string) {
    }
}