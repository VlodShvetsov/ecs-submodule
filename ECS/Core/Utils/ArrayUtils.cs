#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    public interface IArrayElementCopy<T> {

        void Copy(T from, ref T to);
        void Recycle(T item);

    }

    public interface IArrayElementCopyWithIndex<T> {

        void Copy(int index, T from, ref T to);
        void Recycle(int index, ref T item);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class ArrayUtils {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear(System.Array arr) {

            if (arr != null) System.Array.Clear(arr, 0, arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(BufferArray<T> arr) {

            if (arr.arr != null) System.Array.Clear(arr.arr, 0, arr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref ListCopyable<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (item != null) {

                for (int i = 0; i < item.Count; ++i) {

                    copy.Recycle(item[i]);

                }

                PoolListCopyable<T>.Recycle(ref item);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref System.Collections.Generic.List<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (item != null) {

                for (int i = 0; i < item.Count; ++i) {

                    copy.Recycle(item[i]);

                }

                PoolList<T>.Recycle(ref item);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Recycle<T, TCopy>(ref ME.ECS.Collections.BufferArray<T> item, TCopy copy) where TCopy : IArrayElementCopy<T> {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(item.arr[i]);

            }

            PoolArray<T>.Recycle(ref item);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void RecycleWithIndex<T, TCopy>(ref ME.ECS.Collections.BufferArray<T> item, TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> {

            for (int i = 0; i < item.Length; ++i) {

                copy.Recycle(i, ref item.arr[i]);

            }

            PoolArray<T>.Recycle(ref item);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(System.Collections.Generic.List<T> fromArr, ref System.Collections.Generic.List<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    for (int i = 0; i < arr.Count; ++i) {

                        copy.Recycle(arr[i]);

                    }

                    PoolList<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null || fromArr.Count != arr.Count) {

                if (arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = PoolList<T>.Spawn(fromArr.Count);

            }

            var cnt = arr.Count;
            for (int i = 0; i < fromArr.Count; ++i) {

                var isDefault = i >= cnt;
                T item = (isDefault ? default : arr[i]);
                copy.Copy(fromArr[i], ref item);
                if (isDefault == true) {

                    arr.Add(item);

                } else {

                    arr[i] = item;

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(ListCopyable<T> fromArr, ref ListCopyable<T> arr) where T : struct {

            if (fromArr == null) {

                if (arr != null) {

                    PoolListCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null) {

                arr = PoolListCopyable<T>.Spawn(fromArr.Count);

            }

            arr.CopyFrom(fromArr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(CCList<T> fromArr, ref CCList<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    for (int i = 0; i < arr.Count; ++i) {

                        copy.Recycle(arr[i]);

                    }

                    PoolCCList<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr != null) {

                for (int i = 0; i < arr.Count; ++i) {

                    copy.Recycle(arr[i]);

                }

                PoolCCList<T>.Recycle(ref arr);

            }

            arr = PoolCCList<T>.Spawn();
            arr.InitialCopyOf(fromArr);

            for (int i = 0; i < fromArr.array.Length; ++i) {

                if (fromArr.array[i] == null && arr.array[i] != null) {

                    for (int k = 0; k < arr.array[i].Length; ++k) {

                        copy.Recycle(arr.array[i][k]);

                    }

                    PoolArray<T>.Release(ref arr.array[i]);

                } else if (fromArr.array[i] != null && arr.array[i] == null) {

                    arr.array[i] = PoolArray<T>.Claim(fromArr.array[i].Length);

                } else if (fromArr.array[i] == null && arr.array[i] == null) {

                    continue;

                }

                var cnt = fromArr.array[i].Length;
                for (int j = 0; j < cnt; ++j) {

                    copy.Copy(fromArr.array[i][j], ref arr.array[i][j]);

                }

            }

            /*
            if (arr == null || fromArr.Count != arr.Count) {

                if (arr != null) {
                    
                    for (int i = 0; i < arr.Count; ++i) {
                        
                        copy.Recycle(arr[i]);
                        
                    }
                    
                    PoolCCList<T>.Recycle(ref arr);
                    
                }
                
                arr = PoolCCList<T>.Spawn();
                arr.InitialCopyOf(fromArr);

            }

            for (int i = 0; i < fromArr.array.Length; ++i) {

                if (fromArr.array[i] == null && arr.array[i] != null) {
                    
                    for (int k = 0; k < arr.array[i].Length; ++k) {
                        
                        copy.Recycle(arr.array[i][k]);
                        
                    }
                    
                    PoolArray<T>.Release(ref arr.array[i]);
                    
                } else if (fromArr.array[i] != null && arr.array[i] == null) {

                    arr.array[i] = PoolArray<T>.Claim(fromArr.array[i].Length);

                } else if (fromArr.array[i] == null && arr.array[i] == null) {
                    
                    continue;
                    
                }
                
                var cnt = fromArr.array[i].Length;
                for (int j = 0; j < cnt; ++j) {

                    copy.Copy(fromArr.array[i][j], ref arr.array[i][j]);

                }

            }*/

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(ListCopyable<T> fromArr, ref ListCopyable<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr == null) {

                if (arr != null) {

                    for (int i = 0; i < arr.Count; ++i) {

                        copy.Recycle(arr[i]);

                    }

                    PoolListCopyable<T>.Recycle(ref arr);

                }

                arr = null;
                return;

            }

            if (arr == null || fromArr.Count != arr.Count) {

                if (arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = PoolListCopyable<T>.Spawn(fromArr.Count);

            }

            var cnt = arr.Count;
            for (int i = 0; i < fromArr.Count; ++i) {

                var isDefault = i >= cnt;
                T item = (isDefault ? default : arr[i]);
                copy.Copy(fromArr[i], ref item);
                if (isDefault == true) {

                    arr.Add(item);

                } else {

                    arr[i] = item;

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T, TCopy>(ME.ECS.Collections.BufferArray<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr, TCopy copy) where TCopy : IArrayElementCopy<T> {

            if (fromArr.arr == null) {

                if (arr.arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                if (arr.arr != null) ArrayUtils.Recycle(ref arr, copy);
                arr = PoolArray<T>.Spawn(fromArr.Length);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(fromArr.arr[i], ref arr.arr[i]);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyWithIndex<T, TCopy>(ME.ECS.Collections.BufferArray<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr, TCopy copy)
            where TCopy : IArrayElementCopyWithIndex<T> {

            if (fromArr.arr == null) {

                if (arr.arr != null) ArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                if (arr.arr != null) ArrayUtils.RecycleWithIndex(ref arr, copy);
                arr = PoolArray<T>.Spawn(fromArr.Length);

            }

            for (int i = 0; i < fromArr.Length; ++i) {

                copy.Copy(i, fromArr.arr[i], ref arr.arr[i]);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref Unity.Collections.NativeArray<T> arr, bool resizeWithOffset = true,
                                     Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var offset = (resizeWithOffset == true ? 2 : 1);

            if (arr.IsCreated == false) arr = new Unity.Collections.NativeArray<T>(index * offset + 1, allocator);
            if (index < arr.Length) return false;

            var newLength = arr.Length * offset + 1;
            if (newLength == 0 || newLength <= index) newLength = index * offset + 1;

            var newArr = new Unity.Collections.NativeArray<T>(newLength, allocator);
            Unity.Collections.NativeArray<T>.Copy(arr, 0, newArr, 0, arr.Length);
            arr.Dispose();
            arr = newArr;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(Unity.Collections.NativeArray<T> fromArr, ref Unity.Collections.NativeArray<T> arr) where T : struct {

            if (arr == null || arr.IsCreated == false) {

                arr = new Unity.Collections.NativeArray<T>(fromArr.Length, Unity.Collections.Allocator.Persistent, Unity.Collections.NativeArrayOptions.ClearMemory);

            }

            Unity.Collections.NativeArray<T>.Copy(fromArr, arr, fromArr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CopyWithIndex<T, TCopy>(BufferArraySliced<T> fromArr, ref BufferArraySliced<T> arr, TCopy copy)
            where TCopy : IArrayElementCopyWithIndex<T> {

            arr = arr.CopyFrom(in fromArr, copy);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArraySliced<T> fromArr, ref ME.ECS.Collections.BufferArraySliced<T> arr) {

            arr = arr.CopyFrom(in fromArr);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArray<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr) {

            if (fromArr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(fromArr.Length);

            }

            System.Array.Copy(fromArr.arr, arr.arr, fromArr.Length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(in ME.ECS.Collections.BufferArray<T> fromArr, int sourceIndex, ref ME.ECS.Collections.BufferArray<T> arr, int destIndex, int length) {

            if (fromArr.arr == null) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = BufferArray<T>.Empty;
                return;

            }

            if (arr.arr == null || fromArr.Length != arr.Length) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(fromArr.Length);

            }

            System.Array.Copy(fromArr.arr, sourceIndex, arr.arr, destIndex, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(System.Collections.Generic.IList<T> fromArr, ref T[] arr) {

            if (fromArr == null) {

                arr = null;
                return;

            }

            if (arr == null || fromArr.Count != arr.Length) {

                if (arr != null) PoolArray<T>.Recycle(ref arr);
                arr = new T[fromArr.Count];

            }

            fromArr.CopyTo(arr, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Copy<T>(System.Collections.Generic.IList<T> fromArr, ref ME.ECS.Collections.BufferArray<T> arr) {

            if (fromArr == null) {

                if (arr != null) PoolArray<T>.Recycle(ref arr);
                arr = new BufferArray<T>(null, 0);
                return;

            }

            if (arr.arr == null || fromArr.Count != arr.Length) {

                if (arr.arr != null) PoolArray<T>.Recycle(ref arr);
                arr = PoolArray<T>.Spawn(fromArr.Count);

            }

            fromArr.CopyTo(arr.arr, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResize<T>(int index, ref T[] arr) {

            if (arr == null) return true; //arr = PoolArray<T>.Spawn(index + 1);
            if (index < arr.Length) return false;
            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResize<T>(int index, ref BufferArray<T> arr) {

            if (arr.arr == null) return true;
            if (index < arr.Length) return false;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool WillResizeWithBuffer<T>(int index, ref BufferArray<T> arr) {

            if (arr.arr == null) return true;
            if (index < arr.Length) return false;
            var newLength = index + 1;
            if (newLength <= arr.arr.Length) return false;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void RawResize<T>(int index, ref BufferArray<T> arr) {

            var newSize = index * 2 + 1;
            if (arr.arr == null || newSize > arr.arr.Length) {

                var newArr = (T[])arr.arr;
                System.Array.Resize(ref newArr, newSize);
                arr = new BufferArray<T>(newArr, newSize);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref BufferArray<T> arr, bool resizeWithOffset = false) {

            if (index < arr.Length) return false;

            var offset = (resizeWithOffset == true ? 2 : 1);
            if (arr.arr == null) {

                arr = PoolArray<T>.Spawn(index * offset + 1);
                arr = new BufferArray<T>(arr.arr, index + 1);

            }

            var newLength = index + 1;
            if (newLength <= arr.arr.Length) {

                System.Array.Clear(arr.arr, arr.Length, newLength - arr.Length);
                arr = new BufferArray<T>(arr.arr, newLength);
                return false;

            }

            var newArr = PoolArray<T>.Spawn(newLength);
            System.Array.Copy(arr.arr, newArr.arr, arr.Length);
            if (arr != newArr) PoolArray<T>.Recycle(ref arr);
            arr = newArr;

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool Resize<T>(int index, ref BufferArraySliced<T> arr, bool resizeWithOffset = false) {

            arr = arr.Resize(index, resizeWithOffset, out var result);
            return result;

        }

    }

}