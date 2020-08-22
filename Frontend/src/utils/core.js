// https://stackoverflow.com/questions/14446511/most-efficient-method-to-groupby-on-an-array-of-objects

export class Functional {
  static groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
      const key = keyGetter(item);
      const collection = map.get(key);
      if (!collection) {
        map.set(key, [item]);
      } else {
        collection.push(item);
      }
    });
    return map;
  }
}
