
export class DateUtils {

  static MONTHS_SHORT: string[] = [
    "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
  ]


  /**
   * Returns today in the format 'YYYY-MM-DD'.
   * @returns {string} Today in 'YYYY-MM-DD' format.
   */
  static getToday(): string {
    return new Date().toISOString().split('T')[0];
  }

  /**
   * Returns the first day of the current year in the format 'YYYY-MM-DD'.
   * @returns {string} The first day of the current year in 'YYYY-MM-DD' format.
   */
  static getFirstDayOfCurrentYear(): string {
    const now = new Date();
    const firstDayOfYear = new Date(now.getFullYear(), 0, 1); // January 1st
    return firstDayOfYear.toISOString().split('T')[0];
  }

  /**
   * Returns the first day of the current month in the format 'YYYY-MM-DD'.
   * @returns {string} The first day of the current month in 'YYYY-MM-DD' format.
   */
  static getFirstDayOfCurrentMonth(): string {
    const now = new Date();
    const firstDayOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
    return firstDayOfMonth.toISOString().split('T')[0];
  }

  /**
   * Returns the first day of the current week in the format 'YYYY-MM-DD'.
   * Monday is considered the first day of the week.
   * @returns {string} The first day of the current week in 'YYYY-MM-DD' format.
   */
  static getFirstDayOfCurrentWeek(): string {
    const now = new Date();
    const dayOfWeek = now.getDay();
    const diffToMonday = dayOfWeek === 0 ? 6 : dayOfWeek - 1;
    const firstDayOfWeek = new Date(now.setDate(now.getDate() - diffToMonday));
    return firstDayOfWeek.toISOString().split('T')[0];
  }
}
